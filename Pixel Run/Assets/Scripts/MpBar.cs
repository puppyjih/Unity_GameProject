using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MpBar : MonoBehaviour
{
    [SerializeField] private GameObject Target;
    [SerializeField] private Image mpBar;

    private PlayerStat player;
    private int prevMP;

    private void Start()
    {
        player = Target.GetComponent<PlayerStat>();
        prevMP = player.MP;
    }

    void Update()
    {
        if (prevMP != player.MP)
        {
            mpBar.fillAmount = 1f * player.MP / player.MaxMP();
            prevMP = player.MP;
        }
    }
}
