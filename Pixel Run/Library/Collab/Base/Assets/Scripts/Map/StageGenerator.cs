using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    public static StageGenerator instance = null;
    private MapGenerator mapGenerator;
    private readonly string templatePath = "MapTemplates/";
    private Transform mapTransform;
    private Player player;
    private PlayerStat playerStat;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        // 일단은 Map : 4*4 Template : 12 * 9
        mapGenerator = gameObject.GetComponent<MapGenerator>();
        mapTransform = GameObject.Find("MapParent").transform;
        GameObject PlayerGameObject = GameObject.Find("Player");
        player = PlayerGameObject.GetComponent<Player>();
        playerStat = PlayerGameObject.GetComponent<PlayerStat>();
        instance.StageSpawn();
    }


    public void StageSpawn(int stage = 1) {
        if (stage == 1) {
            player.HP = Player.MAXHP;
            playerStat.MP = PlayerStat.MAXMP;
        }
        DestroyStage();
        mapGenerator.MapSpawn(templatePath + "Stage" + stage.ToString() + "/", 10, 10);
    }

    private void DestroyStage() {
        foreach (Transform child in mapTransform) {
            child.gameObject.SetActive(false);
            // GameObject.Destroy(child.gameObject);
        }
    }
}
