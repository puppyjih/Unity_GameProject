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
    private Transform[] mapTransforms;
    private Player player;
    private int mainStage = 1, subStage = 1;
    private int[] subStageCounts = new int[]{4};
    private bool[] goSpecialStage = new bool[]{false};
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        // 일단은 Map : 4*4 Template : 12 * 9
        mapGenerator = gameObject.GetComponent<MapGenerator>();
        GameObject PlayerGameObject = GameObject.Find("Player");
        player = PlayerGameObject.GetComponent<Player>();
        instance.StageSpawn(-1);
    }


    public void StageSpawn(int stageClear) {
        DestroyStage();
        if (stageClear < 0) {
            player.ResetStat();
            mainStage = 1;
            subStage = 1;
        } else if (stageClear > 0) {
            subStage++;
            if (subStage > subStageCounts[mainStage - 1]) {
                mainStage++;
                subStage = 1;
            }
        } else {
            mapGenerator.SpecialStage(templatePath + "Stage" + mainStage.ToString() + "Special/");
            StageView.instance.View("Special Stage 1");
            return;
        }
        if (mainStage > subStageCounts.Length) return; // TODO: go Ending
        bool specialStage = false;
        if (!goSpecialStage[mainStage - 1] && Utility.GetRandomRange(0, subStageCounts[mainStage - 1] - subStage + 1) == 0) {
            specialStage = true;
        }
        mapGenerator.MapSpawn(templatePath + "Stage" + mainStage.ToString() + "/", 4, 4, specialStage);
        StageView.instance.View(mainStage, subStage);

    }

    private void DestroyStage() {
        // set Parent
        GameObject[] GameObjectParent = GameObject.FindGameObjectsWithTag("GameObjectParent");
        mapTransforms = new Transform[GameObjectParent.Length];
        for (int i = 0; i < GameObjectParent.Length; i++) {
            mapTransforms[i] = GameObjectParent[i].transform;
        }
        //
        foreach (Transform parentObject in mapTransforms) {
            foreach (Transform child in parentObject) {
                child.gameObject.SetActive(false);
                // GameObject.Destroy(child.gameObject);
            }
        }
        GameObject.Find("Player").transform.position = new Vector2(-100f, -100f);
        GameObject.Find("Portal").transform.position = new Vector2(-100f, -100f);
        GameObject.Find("GoldenPortal").transform.position = new Vector2(-100f, -100f);
    }
}
