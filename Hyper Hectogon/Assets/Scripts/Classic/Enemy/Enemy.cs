using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour, IPooledObject
{
    // Public Variables
    public int Tier { get; set; }
    public int Score { get; set; }

    // Serialize Fields
    [SerializeField] private Sprite[] enemySprites;
    [SerializeField] private PlayerStat playerStat;

    // Private Variables
    private SpriteRenderer spriteRenderer;
    private int enemySpriteLength;
        
    void Awake()
    {
        Tier = 0;
        Score = 10;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        enemySpriteLength = enemySprites.Length;
    }

    public void OnObjectSpawn()
    {
        int stage = playerStat.Stage;
        Score = (stage + 1) * 20;
        Tier = stage;
        if (stage < enemySpriteLength)
        {
            spriteRenderer.sprite = enemySprites[stage];
        }
        else // USE UNTIL ENEMYSPRITES ARE DRAWN
        {
            spriteRenderer.sprite = enemySprites[enemySpriteLength - 1];
        }
        //spriteRenderer.color = Color.black;

        // Assign Score 
    }

    public int Combat(int playerPower)
    {
        int damage = Tier; // NEEDS MODIFICATION
        damage -= playerPower;

        return damage;
    }
}
