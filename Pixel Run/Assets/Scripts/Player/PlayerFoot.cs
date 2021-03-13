using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoot : MonoBehaviour
{
    private int damage = 10;
    private Rigidbody2D myBody;

    private void Start()
    {
        myBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy")) {
            if (myBody.velocity.y < 0) {
                IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
                if (damagable != null) {
                    Enemy enemy = other.gameObject.GetComponent<Enemy>();
                    if (!enemy.damageActive) return;
                    Vector2 v = myBody.velocity;
                    if (v.y < 0) v.y = 0;
                    myBody.velocity = v + new Vector2(0f, 5f);
                    damagable.GetDamage(damage);
                }
            }
        }
        
        //Debug.Log("myBody : " + myBody.velocity.y);
    }
}
