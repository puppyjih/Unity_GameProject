using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private float __attackSpeed__;
    [SerializeField] private float __spriteAngle__;
    [SerializeField] private string bulletTag; // tag of the bullet
    [SerializeField] Transform spawnPosition;
    public string ownerTag { get; set; }

    ObjectPooler objectPooler;

    public float attackSpeed { get { return __attackSpeed__; } set { __attackSpeed__ = value; } }
    public float spriteAngle { get { return __spriteAngle__; } set { __spriteAngle__ = value; } }

    private void Start()
    {
        attackSpeed = __attackSpeed__;
        objectPooler = ObjectPooler.Instance;
    }

    public void Attack() 
    {
        for (int i = 0; i < 5; ++i)
        {
            objectPooler.SpawnFromPool(bulletTag, spawnPosition.position, Quaternion.identity);
        }
    }
}
