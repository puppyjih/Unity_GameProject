using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    // private variable
    private bool isDead;

    // protected variable
    protected Transform _transform;
    protected bool isTriggered;
    protected float isFlipped;
    protected int meleeDamage;

    // public variable
    public int HP { get; set; }
    public string colliderToIgnore { get; set; }
    public bool damageActive { get; set; }

    protected void Awake()
    {
        damageActive = true;
        isFlipped = gameObject.transform.localScale.x;
        _transform = transform;
        isTriggered = false;
        isDead = false;
        colliderToIgnore = "Enemy";
        meleeDamage = 10;
        HP = 10;

    }

    protected void Update()
    {
        if (isDead) return; // TEMP TEMP TEMP
        if (isTriggered) {
            ActiveMovement();
        } else {
            UnActiveMovement();
            CheckTrigger();
        }
    }

    protected virtual void ActiveMovement() {}
    protected virtual void UnActiveMovement() {}
    protected virtual void CheckTrigger() {}
    /// <summary>
    /// flip enemy
    /// </summary>
    protected void Flip() {
        isFlipped = -isFlipped;
        transform.localScale = new Vector2(isFlipped, 1f);
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Player")) {
            PlayerStat playerStat = other.gameObject.GetComponent<PlayerStat>();
            Player player = other.gameObject.GetComponent<Player>();
            if (!playerStat.isInvincibility) {
                playerStat.GetDamage(meleeDamage);

                int pushDirection = (_transform.position.x - player.transform.position.x) < 0 ? 1 : -1;
                Vector2 pushVector = new Vector2(pushDirection, 0.3f);
                player.BounceOff(pushVector * 10f);
            }
        }
    }

    public void GetDamage(int damage)
    {
        HP -= damage;
        if(HP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;

        // TODO : Animation for die
        gameObject.SetActive(false); // TEMP

        // TODO : Player exp++
    }
}
