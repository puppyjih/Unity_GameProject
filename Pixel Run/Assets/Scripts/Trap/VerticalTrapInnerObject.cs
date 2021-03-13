using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalTrapInnerObject : MonoBehaviour
{
    private int damage = 10;
    private float delayTime = 0.3f;
    private float initPositionY;
    private bool moveActive;
    private int direction;

    // Start is called before the first frame update
    void Start()
    {
        initPositionY = gameObject.transform.position.y;
        direction = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveActive) {
            Vector2 vector = new Vector2(0f, 1f);
            if (direction == -1 || transform.position.y >= initPositionY + 1) {
                vector = new Vector2(0f, -1f);
                direction = -1;
            }
            transform.Translate(vector * Time.deltaTime);
        }
        if (moveActive && direction == -1 && transform.position.y < initPositionY) {
            direction = 1;
            moveActive = false;
        }
    }

    public void ActionStart() {
        StartCoroutine(Delay());
    }

    IEnumerator Delay() {
        yield return new WaitForSeconds(delayTime);
        moveActive = true;
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerStat>().GetDamage(damage);
        }
    }
}
