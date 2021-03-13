using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour, IDamagable
{
    ////////////// private
    private int __HP__;
    private int __MP__;
    private Player player;
    private int[] stat = new int[4];
    private bool __isInvincibility;
    private bool isDead;
    
    ////////////// public
    public readonly static int MAXMP = 100;
    public readonly static int MAXHP = 100;
    public int MP { get { return __MP__; } set { __MP__ = value < 0 ? 0 : value > MaxMP() ? MaxMP() : value; } }
    public int HP { get { return __HP__; } set { __HP__ = value < 0 ? 0 : value > MAXHP ? MAXHP : value; } }

    public bool adhesive { get; set; }
    public bool isInvincibility { get { return __isInvincibility; } }
    public string colliderToIgnore { get; set; }
    // Start is called before the first frame update
    void Awake()
    {
        isDead = false;
        colliderToIgnore = "Player";
    }

    public void ResetStat() {
        adhesive = false;
        MP = MAXMP;
        HP = MAXHP;
        isDead = false;
    }

    public void PayMP(int mana) {
        MP -= mana;
    }

    public void UpdateStat(int key, int value) {
        stat[key] += value;
    }

    public int MaxHP() {
        return MAXHP + stat[(int)StatEnum.AddMaxHP];
    }
    
    public int MaxMP() {
        return MAXMP + stat[(int)StatEnum.AddMaxMP];
    }
    
    public void HealHP() {
        HP += stat[(int)StatEnum.healHP];
    }

    public void HealMP() {
        MP += stat[(int)StatEnum.healMP];
    }

    public void GetDamage(int damage)
    {
        if (!__isInvincibility) {
            HP -= damage;
            StartCoroutine(isInvincibilityOff());
        }
        if (HP <= 0)
        {
            Die();
        }
    }

    IEnumerator isInvincibilityOff() {
        __isInvincibility = true;
        gameObject.layer = 13;
        yield return new WaitForSeconds(1f);
        __isInvincibility = false;
        gameObject.layer = 9;
    }

    public void Die()
    {
        isDead = true;

        // TODO : animation for die
        // gameObject.SetActive(false); // TEMP
        GameObject.Find("DeadUI").transform.GetChild(0).gameObject.SetActive(true);
    }
}
