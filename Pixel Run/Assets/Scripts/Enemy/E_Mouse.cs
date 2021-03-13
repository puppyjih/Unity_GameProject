using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Mouse : Enemy
{
    private Bounds bounds;
    private Rigidbody2D myBody;
    private float moveSpeed = 6f;
    private int _layerMask = 1<<11;

    void Awake()
    {
        base.Awake();
        meleeDamage = 10;
        bounds = GetComponent<BoxCollider2D>().bounds;
        myBody = GetComponent<Rigidbody2D>();
    }

    protected override void UnActiveMovement()
    {
        bool Check(Vector2 start, Vector2 direction, float distance) { // return crash thing (like brick)
            RaycastHit2D[] hits = Physics2D.RaycastAll(start, direction, distance);
            Debug.DrawRay(start, direction * distance, Color.green);
            if (hits.Length == 0) return false;
            int count = 0;
            for (int i = 0; i < hits.Length; i++) {
                if (hits[i].transform.gameObject.GetComponent<Rigidbody2D>() == null && !hits[i].transform.gameObject.CompareTag("LadderBar") && !hits[i].transform.gameObject.CompareTag("Ladder")) count++;
            }
            return count > 0;
        }
        Vector2 front = transform.position + isFlipped * Vector3.right * 0.4f, down = transform.position + (isFlipped * Vector3.right + Vector3.down) * 0.4f;
        bool frontWall = Check(front, Vector2.right * isFlipped, 0.3f);
        bool downBrick = !Check(down, new Vector2(2f * isFlipped, -1f), 0.3f);
        if (!frontWall && !downBrick) {
            myBody.velocity = isFlipped * Vector2.right * moveSpeed;
        } else {
            Flip();
        }
    }
}
