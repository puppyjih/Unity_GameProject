using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // Start is called before the first frame update
    private int damage = 10;

    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Foot")
        {
            Rigidbody2D myBody = collision.GetComponentInParent<Rigidbody2D>();
            if (myBody.velocity.y < -21f)
            {
                IDamagable damagable = myBody.gameObject.GetComponent<IDamagable>();
                if (damagable != null)
                {
                    Vector2 v = myBody.velocity;
                    if (v.y < 0) v.y = 0;
                    myBody.velocity = v + new Vector2(Random.RandomRange(-3f, 3f), 4f);
                    damagable.GetDamage(damage);
                }
            }
        }
    }
}