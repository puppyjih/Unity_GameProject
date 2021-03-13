using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private GameObject Target;
    [SerializeField] private Image hpBar;

    private PlayerStat playerStat;
    private int prevHP;

    private void Start()
    {
        playerStat = Target.GetComponent<PlayerStat>();
        prevHP = playerStat.HP;
    }

    void Update()
    {
        if (prevHP != playerStat.HP)
        {
            hpBar.fillAmount = 1f * playerStat.HP / playerStat.MaxHP(); //TEMP
            prevHP = playerStat.HP;
        }
    }
}
