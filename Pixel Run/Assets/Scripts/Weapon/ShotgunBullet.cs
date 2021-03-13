using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : Bullet
{
    [SerializeField] private Transform player;
    
    public override void DoSomething() 
    {
        myBody.velocity = new Vector2(player.localScale.x * Random.Range(16f, 18f), Random.Range(-4.6f, 4.6f));
    }
}
