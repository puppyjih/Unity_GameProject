using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalTrapInnerObject : MonoBehaviour
{
    private Rigidbody2D myBody;
    private bool shoot;
    private BoxCollider2D box;
    private float speed = 30f;
    private float parentFlip;
    private int damage = 100;
    [SerializeField]
    private PlayerStat playerStat;

    void Start()
    {
        //player = gameObject.GetComponent<Player>();
        myBody = gameObject.GetComponent<Rigidbody2D>();
        box = gameObject.GetComponent<BoxCollider2D>();
        parentFlip = transform.parent.localScale.x;
        shoot = false;
    }

    public void ActionStart() 
    {
        if(myBody == null)
        {
            myBody = gameObject.GetComponent<Rigidbody2D>();
        }
        myBody.velocity = new Vector2(1f, 0f) * speed * parentFlip;
        shoot = true;
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!shoot) return;
        if (other.gameObject.GetComponent<Rigidbody2D>() == null) {
            IWeapon weapon = other.gameObject.GetComponent<IWeapon>();
            if (other.transform.parent.gameObject.CompareTag("Player") || (weapon != null && weapon.ownerTag.Equals("Player")))
            {
                //player.GetDamage(damage);
                playerStat.GetDamage(damage);
                //other.transform.parent.gameObject.GetComponent<Player>().GetDamage(damage);
            }
            gameObject.SetActive(false);
        } else {
            if (other.gameObject.CompareTag("Player")) {
                playerStat.GetDamage(damage);
            } else if (other.gameObject.CompareTag("Enemy")) {
                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                enemy.GetDamage(damage);
            }
        }
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerExit2D(Collider2D other) {
        if (!shoot) return;
        if (other.gameObject == transform.parent.gameObject) {
            //box.isTrigger = false;
        }
    }
}
