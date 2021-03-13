using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour 
{
    // Private Variables
    private int tier = 0;
    private int maxTier = 19;
    private int stage = 0;
    private int maxStage = 19;
    private float health;
    private float maxHealth = 100f;
    private float minHealth = 0f;
    //private int maxAngle = 6;
    //private int minAngle = 3;
    //private int power = 0;
    //private int maxPower = 19;

    // Public Variables
    //public int Angle { get; set; } // 0 ~ 3
    public int Tier { get { return tier > maxTier ? maxTier : tier; } set { tier = value; } }
    public int Stage { get { return stage > maxStage ? maxStage : stage; } set { stage = value; } }
    //public int MaxAngle { get { return maxAngle; } }
    //public int MinAngle { get { return minAngle; } }
    //public int MaxTier { get { return maxTier; } }
    //public int Power { get { return power > maxPower ? maxPower : power; } set { maxPower = value; } }
    public float Health { get { return health > maxHealth ? maxHealth : minHealth > health ? minHealth : health; } set { health = value; } } // range{0, 100}

    // UI
    private float beforeHp = 0f;
    [SerializeField] private Slider HPBar;

    private void Start()
    {
        //Angle = 0;
        Tier = 0;
        Health = 100f;
        Stage = 0;
    }

    private void Update()
    {
        // Health Bar Update
        if (beforeHp != Health)
        {
            beforeHp = Health;
            HPBar.value = Health;
        }
    }
}