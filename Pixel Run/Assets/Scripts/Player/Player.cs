using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Joystick leftJoystick; // controls player left, right movement
    [SerializeField] private GameObject defaultWeaponObj;
    private GameObject weaponObject;
    private IWeapon defaultWeapon;
    private IWeapon weapon;
    private Throwable throwable;
    private Transform Hand;
    private Rigidbody2D myBody;
    private GameObject lastCrashObject = null;
    private Transform _transform; // cached transform
    private Camera _camera;
    private Bounds myBounds;
    private Vector2 _moveVector;
    private float jumpForce = 11f;
    private float moveSpeed = 4f;
    private float defaultSpeed = 1f;
    private float runSpeed = 2f;
    private bool isAttacking;
    private bool isControlable;
    private int isLookingRight = 1;
    private int ignoreJumpLayermask = ~(1 << 8);
    private LadderMovement ladderMovement;
    private Inventory inventory;
    private System.Diagnostics.Stopwatch watch;
    private bool isWatchWorking = false;
    private CameraShake vcam;
    private float flip = 1f;
    private HangOn hangOn;
    private PlayerStat playerStat;

    public Vector2 leftJoystickVector { get { return _moveVector; } }
    public float isFlip { get { return flip; } }


    void Start()
    {
        _moveVector = Vector2.zero;
        _camera = Camera.main;
        _transform = transform; // Transform caching
        Hand = transform.Find("Hand");
        myBody = gameObject.GetComponent<Rigidbody2D>();
        defaultWeapon = defaultWeaponObj.GetComponent<IWeapon>();
        myBounds = gameObject.GetComponent<BoxCollider2D>().bounds;
        ladderMovement = gameObject.GetComponent<LadderMovement>();
        inventory = gameObject.GetComponent<Inventory>();

        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        vcam = GameObject.Find("ControlCameraShake").GetComponent<CameraShake>();
        playerStat = gameObject.GetComponent<PlayerStat>();
        hangOn = gameObject.GetComponent<HangOn>();
        Physics2D.queriesStartInColliders = false;
        ladderMovement.onLadder = false;
        ResetStat();
    }

    public void ResetStat() {
        if (playerStat == null) Start();
        isAttacking = false;
        isControlable = true;
        playerStat.ResetStat();
    }

    void Update()
    {
        //if(Input.GetMouseButton(0))
        //{

        //    leftJoystick.SetPosition(Input.mousePosition);
        //}
        TouchInput();
        HandleInput();
    }

    void FixedUpdate()
    {
        if (ladderMovement.onLadder)
        {
            ladderMovement.LadderMove();
        }
        else if(isControlable)
        {
            Move();
        }
        
        if (!ladderMovement.onLadder && !hangOn.isHangOn) {
            hangOn.Enter(isFlip);
        }
        
    }

    private void TouchInput()
    {
        for (int i = 0; i < Input.touchCount; ++i)
        {
            Touch touch = Input.GetTouch(i);
            Vector2 touchPosition = _camera.ScreenToViewportPoint(touch.position);
            if (touchPosition.x < 0.5f)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    leftJoystick.batchJoystick(touch.position, i);
                }
                else if ((touch.position != Vector2.zero && Input.GetTouch(i).phase == TouchPhase.Moved) || Input.GetMouseButton(0))
                {
                    leftJoystick.pushJoystick(touch.position);
                    MoveCamera();
                }
            }
            if (Input.GetTouch(i).phase == TouchPhase.Ended && leftJoystick.GetTouchNumber() == i)
            {
                leftJoystick.releaseJoystick(touch.position);
                MoveCamera();
            }
        }

        Vector2 mousePosition = _camera.ScreenToViewportPoint(Input.mousePosition);
        if (mousePosition.x < 0.5f && Input.touchCount == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                leftJoystick.batchJoystick(Input.mousePosition, -1);
            }
            else if (Input.GetMouseButton(0))
            {
                leftJoystick.pushJoystick(Input.mousePosition);
                MoveCamera();
            }
        }
        if (Input.GetMouseButtonUp(0) && leftJoystick.GetTouchNumber() == -1)
        {
            leftJoystick.releaseJoystick(Input.mousePosition);
            MoveCamera();
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            leftJoystick.pushJoystick(new Vector2((leftJoystick.transform.position.x - 200),leftJoystick.transform.position.y));
            MoveCamera();
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            leftJoystick.pushJoystick(new Vector2((leftJoystick.transform.position.x + 200), leftJoystick.transform.position.y));
            MoveCamera();
        }
        else if(Input.GetKey(KeyCode.UpArrow))
        {
            leftJoystick.pushJoystick(new Vector2((leftJoystick.transform.position.x), leftJoystick.transform.position.y + 200));
            MoveCamera();
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            leftJoystick.pushJoystick(new Vector2((leftJoystick.transform.position.x), leftJoystick.transform.position.y - 200));
            MoveCamera();
        }

        if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            leftJoystick.releaseJoystick(Vector2.zero);
            MoveCamera();
        }

        // Because PC
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Button_Jump();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Button_Attack();
        }
        
        if (Input.GetKey(KeyCode.LeftShift) && myBody.velocity.y == 0f) {
            Run();
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            Walk();   
        }

        if (leftJoystick.GetHorizontalValue() > 0f) {
            flip = 1f;
        } else if (leftJoystick.GetHorizontalValue() < 0f) {
            flip = -1f;
        }
        _transform.localScale = new Vector2(flip, 1f);
    }

    private float Run() {
        return runSpeed;
    }

    private float Walk()
    {
        return defaultSpeed;
    }

    private void HandleInput()
    {
        _moveVector = PoolInput(leftJoystick);
    }

    private Vector2 PoolInput(Joystick joystick)
    {
        float h = joystick.GetHorizontalValue();
        float v = joystick.GetVerticalValue();
        Vector2 moveDir = new Vector2(h, v).normalized;

        return moveDir;
    }

    private void Move()
    {
        if (hangOn.isHangOn) return;
        if (Mathf.Abs(_moveVector.x) > 0)
        {
            int v_x = 0, v_y = 0;
            //if (_moveVector.y >= Mathf.Sin(Mathf.Deg2Rad * 67.5f))
            //    v_y = 4;
            //else if (_moveVector.y >= Mathf.Sin(Mathf.Deg2Rad * 22.5f))
            //    v_y = 3;
            //else if (_moveVector.y >= Mathf.Sin(Mathf.Deg2Rad * -22.5f))
            //    v_y = 2;
            //else if (_moveVector.y >= Mathf.Sin(Mathf.Deg2Rad * 67.5f))
            //    v_y = 1;
            //else
            //    v_y = 0;

            if (Mathf.Abs(_moveVector.x) >= Mathf.Cos(Mathf.Deg2Rad * 22.5f))
                v_x = 2;
            else if (Mathf.Abs(_moveVector.x) >= Mathf.Cos(Mathf.Deg2Rad * 67.5f))
                v_x = 1;
            else
                v_x = 0;
            

            float v_x_bias = Mathf.Abs(leftJoystick.GetHorizontalValue()) >= 0.8 ? Run() : Walk();
            int isFlip = (leftJoystick.GetHorizontalValue() > 0 ? 1 : -1);

            float[] arr_x = { 0f, 0.7f, 1f };
            float[] arr_y = { myBody.velocity.y, myBody.velocity.y, myBody.velocity.y, 1f, 1f };
            Vector2 moveVector = new Vector2(arr_x[v_x] * v_x_bias * isFlip, myBody.velocity.y);
            moveVector.x = moveVector.x * moveSpeed;
            myBody.velocity = moveVector;
        }
        else
            myBody.velocity = new Vector2(0, myBody.velocity.y);
    }

    public void Button_Jump()
    {
        if (hangOn.isHangOn) {
            if (IsPressingDown()) hangOn.Exit(-1f);
            else hangOn.Exit(1f);
        } else if (ladderMovement.onLadder) {
            if (IsPressingDown()) {
                myBody.gravityScale = 3.3f;
            } else {
                myBody.gravityScale = 3.3f;
                myBody.velocity = new Vector2(0f, jumpForce);
            }
            ladderMovement.onLadder = false;
        } else if (IsGrounded()) {
            //Vector2 jumpVector = new Vector2(myBody.velocity.x, _moveVector.y > 0 ? _moveVector.y * jumpForce : myBody.velocity.y);
            Vector2 jumpVector = new Vector2(myBody.velocity.x, jumpForce);
            myBody.velocity = jumpVector;
        }
    }

    private bool IsGrounded()
    {
        Vector2 leftRayPosition = new Vector2(gameObject.transform.position.x - myBounds.extents.x, gameObject.transform.position.y);
        Vector2 rightRayPosition = new Vector2(gameObject.transform.position.x + myBounds.extents.x, gameObject.transform.position.y);

        RaycastHit2D leftHit = Physics2D.Raycast(leftRayPosition, Vector2.down, 0.55f, ignoreJumpLayermask);
        RaycastHit2D rightHit = Physics2D.Raycast(rightRayPosition, Vector2.down, 0.55f, ignoreJumpLayermask);

        Debug.DrawRay(leftRayPosition, Vector2.down * 0.55f, Color.red);

        if (leftHit.collider != null || rightHit.collider != null)
        {
            return true;
        }
        return false;
    }

    public void Button_Attack()
    {
        if (IsPressingDown()) {
            EquipCheck();
        } else if (weapon != null) {
            if (!isAttacking)
            {
                weapon.Attack();
                isAttacking = true;
                //isControlable = false;
                StartCoroutine(Attacking(weapon));
            }
        } else if (throwable != null) {
            throwable.Throw(myBody.velocity, flip);
            Hand.GetChild(0).parent = null;
            throwable = null;
        } else if (defaultWeapon != null) {
            if (!isAttacking) {
                defaultWeapon.Attack();
                isAttacking = true;
                StartCoroutine(Attacking(defaultWeapon));
            }
        }
    }

    /// <summary>
    /// Forces Player velocity to direction param
    /// </summary>
    /// <param name="direction">Set player's velocity to this vector</param>
    public void BounceOff(Vector2 direction)
    {
        if (hangOn.isHangOn) hangOn.Exit(-1);
        isControlable = false;
        myBody.velocity = direction;
        StartCoroutine("TurnControlable");
    }

    IEnumerator TurnControlable()
    {
        yield return new WaitForSeconds(1f);
        isControlable = true;
    }

    private void OnCollisionEnter2D(Collision2D other) {}

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.name.Equals("h_LadderBar"))
        {
            if (IsPressingDown())
            {
                ladderMovement.EnterLadder(other.gameObject);
                other.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            if (IsPressingUp())
            {
                ladderMovement.EnterLadder(other.gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Throwable throwable = other.gameObject.GetComponent<Throwable>();
        if (other.gameObject.CompareTag("Ladder"))
        {
            if (IsPressingUp())
            {
                ladderMovement.EnterLadder(other.gameObject);
            }
        } else if (other.gameObject.CompareTag("Weapon") || throwable != null) {
            if (Hand.childCount > 0 && other.gameObject == Hand.GetChild(0).gameObject) return;
            lastCrashObject = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("h_LadderBar"))
        {
            other.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
        else if (other.gameObject.CompareTag("Ladder"))
        {
            ladderMovement.ExitLadder();
        }
    }

    private IEnumerator Attacking(IWeapon attakWeapon)
    {
        yield return new WaitForSeconds(attakWeapon.attackSpeed);
        isAttacking = false;
        //isControlable = true;
    }

    private void EquipCheck()
    {
        Func<float, float> Abs = delegate (float t) {
            return t > 0f ? t : -t;
        };
        Func<float, float> Pow = delegate (float t) {
            return t * t;
        };
        void UnEquip(ref Transform obj, float flip) {
            Rigidbody2D rb = obj.gameObject.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.constraints = RigidbodyConstraints2D.None;
            obj.parent = null;
            rb.velocity = new Vector2(flip * 3f, 1f);
            if (weapon != null) {
                weapon.ownerTag = "";
                weapon = null;
            } else if (throwable != null) {
                throwable = null;
            }
        }
        Transform myObject = Hand.childCount > 0 ? Hand.GetChild(0) : null;
        if (lastCrashObject == null || Pow(_transform.position.x - lastCrashObject.transform.position.x) + Pow(_transform.position.y - lastCrashObject.transform.position.y) > 2f) {
            lastCrashObject = null;
            if (myObject != null) {
                UnEquip(ref myObject, flip);
            }
            return;
        }
        Vector2 pos = Vector2.zero;
        Throwable gotThrowable = lastCrashObject.GetComponent<Throwable>();
        IWeapon gotWeapon = lastCrashObject.GetComponent<IWeapon>();
        if (myObject != null)
        {
            MeleeWeapon mw = myObject.GetComponent<MeleeWeapon>();
            if (mw != null) {
                mw.SetIsEquiped(false);
            }
            UnEquip(ref myObject, flip);
        }
        lastCrashObject.transform.position = Hand.position;
        lastCrashObject.transform.parent = Hand;
        lastCrashObject.transform.rotation = Quaternion.identity;
        if (gotThrowable != null) {
            throwable = gotThrowable;
            weapon = null;
        } else if (gotWeapon != null) {
            throwable = null;
            gotWeapon.ownerTag = gameObject.tag;
            weapon = gotWeapon;
            MeleeWeapon mw = lastCrashObject.GetComponent<MeleeWeapon>();
            if (mw != null) {
                mw.SetIsEquiped(true);
            }
        } else {
            //
        }
        Rigidbody2D newObjectRB = lastCrashObject.GetComponent<Rigidbody2D>();
        newObjectRB.bodyType = RigidbodyType2D.Kinematic;
        newObjectRB.constraints = RigidbodyConstraints2D.FreezeAll;
        if (myObject != null) {
            lastCrashObject = myObject.gameObject;
        }
        Hand.GetChild(0).localScale = new Vector2(1f, 1f);
        lastCrashObject = null;
        inventory.UpdateWeapon();
    }

    private void MoveCamera()
    {
        if (leftJoystick.GetVerticalValue() > 0.8 && myBody.velocity == Vector2.zero && ladderMovement.onLadder == false)
        {

            if (isWatchWorking == false)
            {
                watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                isWatchWorking = true;
            }
            else if (watch.ElapsedMilliseconds >= 700)
            {
                vcam.LookUp();
            }
        }
        else if (leftJoystick.GetVerticalValue() < -0.8 && myBody.velocity == Vector2.zero && ladderMovement.onLadder == false)
        {
            if (isWatchWorking == false)
            {
                watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                isWatchWorking = true;
            }
            else if (watch.ElapsedMilliseconds >= 700)
            {
                vcam.LookDown();
            }
        }
        else
        {
            if (isWatchWorking == true)
            {
                isWatchWorking = false;
                vcam.Normalize();
                watch.Stop();
            }
        }
    }


    public bool IsPressingUp()
    {
        if (leftJoystick.GetVerticalValue() > 0.8f)
        {
            return true;
        }
        return false;
    }

    public bool IsPressingDown()
    {
        if (leftJoystick.GetVerticalValue() < -0.8f)
        {
            return true;
        }
        return false;
    }

    public bool IsPressingRight() {
        if (leftJoystick.GetHorizontalValue() > 0.8f) {
            return true;
        }
        return false;
    }

    public bool IsPressingLeft() {
        if (leftJoystick.GetHorizontalValue() < -0.8f) {
            return true;
        }
        return false;
    }

    public float getJumpForce()
    {
        return jumpForce;
    }
}