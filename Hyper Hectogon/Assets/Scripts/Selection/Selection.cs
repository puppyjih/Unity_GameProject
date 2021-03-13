using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SelectionObject 
{
    [field: SerializeField]
    public float Percentage { get; set; }

    [field: SerializeField]
    public string Tag { get; set; }
}

public class Selection : MonoBehaviour, IPooledObject
{
    [SerializeField] SelectionObject[] selectionObject;

    ObjectPooler objectPooler;
    private int selectionObjectLength;
    private PlayerStat playerStat;
    private float[, ] delta = new float[19, 4]{{0f, 0f, 0f, 0f}, {0f, 0f, 0f, 0f}, {-0.015f, -0.015f, 0.015f, 0.015f}, {-0.015f, 0f, 0.015f, 0f}, {-0.03f, -0.015f, 0.03f, 0.015f}, 
        {-0.03f, -0.015f, 0.03f, 0.015f}, {-0.03f, -0.015f, 0.03f, 0.015f}, {-0.03f, -0.015f, 0.03f, 0.015f}, {-0.03f, -0.015f, 0.03f, 0.015f}, {-0.03f, -0.015f, 0.03f, 0.015f}, {-0.03f, -0.015f, 0.03f, 0.015f}, 
        {-0.03f, -0.015f, 0.03f, 0.015f}, {-0.03f, -0.015f, 0.03f, 0.015f}, {-0.03f, -0.015f, 0.03f, 0.015f}, {-0.03f, -0.015f, 0.03f, 0.015f}, {-0.03f, -0.015f, 0.03f, 0.015f}, {-0.03f, -0.015f, 0.03f, 0.015f}, 
        {-0.03f, -0.015f, 0.03f, 0.015f}, {-0.03f, -0.015f, 0.03f, 0.015f} };

    void Awake()
    {
        objectPooler = ObjectPooler.Instance;
        selectionObjectLength = selectionObject.Length;
        playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();
    }

    public void OnObjectSpawn()
    {
        string tag = "";

        //Spawn Enemy, PowerUps, Obstacles
        float random = Random.Range(0f, 1f);
        float currentPercentage = 0;

        for(int i = 0; i < selectionObjectLength; i++) 
        {
            currentPercentage += selectionObject[i].Percentage;/* + delta[playerStat.Stage, i];*/
            if (random <= currentPercentage) 
            {
                tag = selectionObject[i].Tag;
                break;
            }
        }

        objectPooler.SpawnFromPool(tag, transform.position, Quaternion.identity);
    }
}