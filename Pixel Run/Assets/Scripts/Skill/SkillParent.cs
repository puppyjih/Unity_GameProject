using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillParent : MonoBehaviour
{
    private bool isWaiting = false;
    private float coolTime;
    private int cost;
    protected GameObject player;
    protected PlayerStat playerStat;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStat = player.GetComponent<PlayerStat>();
    }

    public virtual void Activate() { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cost">Returns the decimal value of mana cost to parameter</param>
    public void UseSkill(out int cost)
    {
        if (!isWaiting && playerStat.MP >= this.cost)
        {
            cost = this.cost;
            isWaiting = true;
            Activate();
            StartCoroutine(waitCooltime());
        }
        else
        {
            cost = 0;
        }
    }
 
    protected void SetCost(int cost)
    {
        this.cost = cost;
    }

    public int GetCost()
    {
        return cost;
    }

    protected void SetCoolTime(float time)
    {
        this.coolTime = time;
    }

    IEnumerator waitCooltime()
    {
        yield return new WaitForSeconds(coolTime);
        isWaiting = false;
    }
}
