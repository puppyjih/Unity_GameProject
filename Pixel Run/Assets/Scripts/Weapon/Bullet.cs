using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bullet : MonoBehaviour, IPooledObject
{
    private int damage = 10;
    private string[] targetTag = { "Enemy", "Treasure" };
    protected Rigidbody2D myBody;
    public string ownerTag = "Player";

    private void Awake()
    {
        myBody = gameObject.GetComponent<Rigidbody2D>();
    }

    public void OnObjectSpawn()
    {
        DoSomething();
    }

    public virtual void DoSomething() { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(targetTag.Contains(other.tag))
        {
            IDamagable damagable = other.GetComponent<IDamagable>();
            if(damagable != null)
            {
                damagable.GetDamage(damage);
                if (ownerTag.Equals("Player") && !other.gameObject.active && other.gameObject.CompareTag("Enemy")) {
                    GameObject obj = GameObject.Find("Player");
                    if (obj != null) {
                        PlayerStat playerStat = obj.GetComponent<PlayerStat>();
                        playerStat.HealMP();
                    }
                }
            }
            gameObject.SetActive(false);
        }

        if(other.CompareTag("Environment"))
        {
            gameObject.SetActive(false);
        }

    }
}
