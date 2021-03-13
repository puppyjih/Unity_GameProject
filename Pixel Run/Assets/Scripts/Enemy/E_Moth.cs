using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Moth : Enemy
{
    private Rigidbody2D myBody;
    private Transform player;
    private bool going, isChasing;
    private Vector3 myHome;
    private float moveSpeed;
    void Awake()
    {
        base.Awake();
        moveSpeed = 3f;
        meleeDamage = 10;
        myBody = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        going = false;
        isChasing = false;
    }

    private void Rotate(Vector3 goVector, float rotateAngle = 0f) {
        float rotZ = Mathf.Atan2(goVector.x, goVector.y) * Mathf.Rad2Deg;
        myBody.MoveRotation(-rotZ + rotateAngle);
    }

    protected override void ActiveMovement() {
        Vector3 GetMoveUnitVector(Vector3 from, Vector3 to) {
            Vector3 moveVector = to - from;
            return moveVector / moveVector.magnitude;
        }

        // 일단 쫓는 행동은 플레이어가 나보다 아래에 올 때에만 가능하다. 그 이후는 상관없음
        if (_transform.position.y >= player.position.y)
            isChasing = true;

        if (isChasing && Utility.DistanceIn(_transform.position, player.position, 10f)) {
            Vector3 goVector = GetMoveUnitVector(transform.position, player.position);
            Rotate(goVector, 180f);
            myBody.velocity = goVector * moveSpeed;
            going = false;
        } else {
            Vector2 clampPosition = Utility.ClampPositionToDot5(_transform.position);
            Debug.DrawRay(clampPosition, Vector2.up * 100f, Color.green);
            if (!going) {
                RaycastHit2D hits = Physics2D.Raycast(clampPosition, Vector2.up, 100f, (1 << 11));
                if (hits.collider == null) return;
                if (hits.transform.gameObject.CompareTag("Environment")) {
                    going = true;
                    myHome = hits.transform.position + Vector3.down;
                }
            } else {
                if (Utility.DistanceIn(_transform.position, myHome, 0.1f)) {
                    myBody.velocity = Vector2.zero;
                    myBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                    going = false;
                    isTriggered = false;
                } else if (Utility.DistanceIn(_transform.position, myHome, 0.4f)) {
                    Rotate(Vector2.up);
                } else {
                    Rotate(myHome - _transform.position);
                    myBody.velocity = GetMoveUnitVector(transform.position, myHome);
                }
            }
        }
    }

    protected override void UnActiveMovement() {}
    protected override void CheckTrigger() {
        if (transform.position.y >= player.position.y && Utility.DistanceIn(_transform.position, player.position, 13f)) {
            myBody.constraints = RigidbodyConstraints2D.None;
            isTriggered = true;
        }
    }
}
