using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Mole : Enemy
{
    [SerializeField] GameObject RockPile;
    private Bounds bounds;
    private Rigidbody2D myBody;
    private Transform player;
    private bool isUnderGround;
    private BoxCollider2D boxCollider2D;
    private float initY;
    private float moveSpeed = 6f;
    private bool goUnder = false;
    private bool isShoot = false;
    private float shootY;

    new void Awake()
    {
        base.Awake();
        isUnderGround = true;
        meleeDamage = 10;
        boxCollider2D = GetComponent<BoxCollider2D>();
        bounds = GetComponent<BoxCollider2D>().bounds;
        myBody = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        damageActive = false;
    }

    protected override void ActiveMovement() {
        if (isShoot)
        {
            return;
        }
        bool Check(Vector2 start, Vector2 direction, float distance) { // return crash thing (like brick)
            RaycastHit2D[] hits = Physics2D.RaycastAll(start, direction, distance, (1 << 11));
            Debug.DrawRay(start, direction * distance, Color.green);
            if (hits.Length == 0) return false;
            int count = 0;
            for (int i = 0; i < hits.Length; i++) {
                if (hits[i].transform.gameObject.GetComponent<Rigidbody2D>() == null && !hits[i].transform.gameObject.CompareTag("LadderBar") && !hits[i].transform.gameObject.CompareTag("Ladder")) count++;
            }
            return count > 0;
        }
        Vector2 clampPosition = Utility.ClampPositionToDot5(transform.position);
        if (isUnderGround) {
            if (transform.position.y >= initY + 1f) {
                myBody.bodyType = RigidbodyType2D.Dynamic;
                shootY = 1f;
                if (Check(clampPosition, Vector2.up, 1f) || Check(clampPosition + Vector2.right * isFlipped, Vector2.up, 1f)) {
                    shootY = 0f;
                }
                damageActive = true;
                boxCollider2D.isTrigger = false;
                isShoot = true;
                myBody.velocity = Vector2.zero;
                StartCoroutine(Shoot());
                isUnderGround = false;
            } else {
                myBody.velocity = Vector2.up * 10f;
            }
        } else {
            // if (!Utility.DistanceIn(_transform.position, player.position, 10)) {
            if (myBody.velocity.y == 0f) {
                if (goUnder) return;
                Vector2 front = transform.position + isFlipped * Vector3.right * 0.4f, downFlip = transform.position + (isFlipped * Vector3.right + Vector3.down) * 0.4f, down = Utility.ClampPositionToDot5(transform.position);
                bool frontWall = Check(front, Vector2.right * isFlipped, 0.3f);
                bool downFlipBrick = Check(downFlip, new Vector2(2f * isFlipped, -1f), 0.3f);
                bool downBrick = Check(down, Vector2.down, 1f);
                // if (!frontWall && !downBrick) {
                //     myBody.velocity = isFlipped * Vector2.right * moveSpeed;
                if (downBrick) {
                    goUnder = true;
                    StartCoroutine(GoUnderGround());
                } else {
                    Flip();
                }
            }
            // } else {
                // bool Check(Vector2 start, Vector2 direction, float distance) { // return crash thing (like brick)
                //     RaycastHit2D[] hits = Physics2D.RaycastAll(start, direction, distance, (1 << 11));
                //     Debug.DrawRay(start, direction * distance, Color.green);
                //     if (hits.Length == 0) return false;
                //     int count = 0;
                //     for (int i = 0; i < hits.Length; i++) {
                //         if (hits[i].transform.gameObject.GetComponent<Rigidbody2D>() == null && !hits[i].transform.gameObject.CompareTag("LadderBar") && !hits[i].transform.gameObject.CompareTag("Ladder")) count++;
                //     }
                //     return count > 0;
                // }
                // Vector2 front = transform.position + isFlipped * Vector3.right * 0.4f, down = transform.position + (isFlipped * Vector3.right + Vector3.down) * 0.4f;
                // bool frontWall = Check(front, Vector2.right * isFlipped, 0.3f);
                // bool downBrick = !Check(down, new Vector2(2f * isFlipped, -1f), 0.3f);
                // if (!frontWall && !downBrick) {
                //     // myBody.velocity = isFlipped * Vector2.right * moveSpeed;
                // } else {
                //     Flip();
                // }
            // }
        }
    }

    IEnumerator Shoot() {
        RockPile.SetActive(false);
        // int cnt = 3;
        // while (cnt-- > 0) {
            yield return new WaitForSeconds(0.2f);
            myBody.velocity = new Vector2(isFlipped * 15f, shootY);
            yield return new WaitForSeconds(3f);
        // }
        isShoot = false;
    }

    IEnumerator GoUnderGround() {
        initY = transform.position.y;
        myBody.bodyType = RigidbodyType2D.Kinematic;
        myBody.velocity = Vector2.zero;
        while (initY - 1 <= _transform.position.y) {
            transform.position += new Vector3(0f, -0.03f, 0f);
            yield return new WaitForSeconds(0.0003f);
        }
        transform.position = Utility.ClampPositionToDot5(transform.position);
        myBody.velocity = Vector2.zero;
        RockPile.SetActive(true);
        boxCollider2D.isTrigger = true;
        isUnderGround = true;
        isTriggered = false;
        damageActive = false;
        goUnder = false;
    }

    protected override void CheckTrigger() {
        float startX = 1.5f, lengthX = 4f;
        float[] vectorX = new float[2]{-1, 1};
        Vector2 clampPosition = Utility.ClampPositionToDot5(transform.position);
        for (int v = 0; v < vectorX.Length; v++) {
            RaycastHit2D[] hits = Physics2D.RaycastAll(clampPosition + Vector2.up + Vector2.right * vectorX[v] * startX, Vector2.right * vectorX[v], lengthX);
            Debug.DrawRay(clampPosition + Vector2.up + Vector2.right * vectorX[v] * startX, Vector2.right * vectorX[v] * lengthX, Color.green);
            if (hits.Length == 0) continue;
            for (int i = 0; i < hits.Length; i++) {
                if (hits[i].transform.gameObject.CompareTag("Player")) {
                    isTriggered = true;
                    initY = clampPosition.y;
                    isFlipped = vectorX[v];
                    _transform.localScale = new Vector2(isFlipped, 1f);
                    return;
                }
            }
        }
    }
}
