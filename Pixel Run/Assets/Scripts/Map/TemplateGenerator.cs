using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ForMapping<T>
{
    public T Key;
    public GameObject Value;
    public ForMapping(T __Key__, GameObject __Value__) 
    {
        Key = __Key__;
        Value = __Value__;
    }
}
public class TemplateGenerator : MonoBehaviour
{
    private class SpawnedPositionType {
        public int x;
        public int y;
        public int type;
        public SpawnedPositionType(int __x__, int __y__, int __type__) {
            x = __x__;
            y = __y__;
            type = __type__;
        }
    }
    [System.Serializable]
    private class ForMappingChar : ForMapping<char> {
        public ForMappingChar(char __Key__, GameObject __Value__) : base(__Key__, __Value__) {}
    }

    [SerializeField] private ForMappingChar[] forMapping;
    private Dictionary<char, GameObject> objectMap;
    private float space = 1f;
    private Transform mapParent;
    private int additionalObstaclesR = 3, additionalObstaclesC = 6;
    private string[] additionalObstacles = new string[]{
        "000111000101000111",
        "000011000111001111",
        "01H11101h00101H111",
        "V1111111101111111V"
    };
    
    void Awake()
    {
        mapParent = GameObject.Find("MapParent").transform;
        Mapping();
    }

    GameObject CreateLadderParent(Vector2 position = new Vector2()) {
        GameObject LadderParent = new GameObject("LadderParent");
        LadderParent.AddComponent<BoxCollider2D>();
        LadderParent.transform.parent = mapParent;
        LadderParent.transform.position = position;
        LadderParent.gameObject.tag = "Ladder";
        return LadderParent;
    }

    private void Mapping()
    {
        objectMap = new Dictionary<char, GameObject>();
        for (int i = 0; i < forMapping.Length; i++)
        {
            objectMap.Add(forMapping[i].Key, forMapping[i].Value);
        }
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="filePath">filePath</param>
    /// <param name="position">spawn position pivot</param>
    /// <param name="type">0 => basic, 1 => entrance, 2 => exit</param>
    public void Spawn(string filePath, Vector2 position, int type, out string[] retTemplate)
    {
        Vector2Int impossiblePosition = new Vector2Int(-1, -1);
        string[] template = GetFileRandom(filePath);
        retTemplate = template;
        if (template.Length == 0) return;
        GameObject[, ] objs = new GameObject[template.Length, template[0].Length];
        float initX = position.x;
        GameObject obj;
        ModifyTemplate(ref retTemplate);
        for(int i = 0; i < template.Length; ++i)
        {
            for (int j = 0; j < template[i].Length; ++j)
            {
                obj = objectMap[template[i][j]];
                Probabilistic probabilistic = obj.GetComponent<Probabilistic>();
                if (probabilistic != null)
                {
                    obj = probabilistic.GetObject();
                }
                obj = Instantiate(obj, position, Quaternion.identity);
                obj.name = objectMap[template[i][j]].name;
                if (j == 0 || template[i][j] == 'V' && template[i][j - 1] == '0') obj.transform.localScale = new Vector3(-1f, 1f, 1f);
                obj.transform.parent = mapParent;
                objs[i, j] = obj;
                position.x += space;
            }
            position.y -= space;
            position.x = initX;
        }
        // MakeLadder
        MakeLadder(template, position, objs);
    }

    private string[] GetFileRandom(in string filePath) {
        TextAsset[] files = Resources.LoadAll<TextAsset>(filePath);
        TextAsset file = files[UnityEngine.Random.Range(0, files.Length)];
        return file.text.Split('\n');
    }

    private void ModifyTemplate(ref string[] template) {
        int[, ] collisionCounter = new int[template.Length, template[0].Length];
        for (int i = 0; i < template.Length; i++) {
            for (int j = 0; j < template[0].Length; j++) {
                int add = 0;
                if (i > 0) add += collisionCounter[i - 1, j];
                if (j > 0) add += collisionCounter[i, j - 1];
                if (i > 0 && j > 0) add -= collisionCounter[i - 1, j - 1];
                collisionCounter[i, j] = add + (template[i][j] == '0' ? 1 : 0);
            }
        }
        int sX = -1;
        int sY = -1;
        for (int i = additionalObstaclesR + 1; i < template.Length; i++) {
            for (int j = additionalObstaclesC + 1; j < template[0].Length; j++) {
                int sum = collisionCounter[i, j];
                if (i - additionalObstaclesR - 2 >= 0) sum -= collisionCounter[i - additionalObstaclesR - 2, j];
                if (j - additionalObstaclesC - 2 >= 0) sum -= collisionCounter[i, j - additionalObstaclesC - 2];
                if (i - additionalObstaclesR - 2 >= 0 && j - additionalObstaclesC - 2 >= 0) sum += collisionCounter[i - additionalObstaclesR - 2, j - additionalObstaclesC - 2];
                if (sum == (additionalObstaclesR + 2) * (additionalObstaclesC + 2)) {
                    if (Utility.GetRandomRange(0, 3) == 0) {
                        sX = i - additionalObstaclesR;
                        sY = j - additionalObstaclesC;
                        break;
                    }
                }
            }
        }
        if (sX == -1 || sY == -1) return;
        string replaceTemplate = Utility.GetRandom(additionalObstacles);
        for (int r = sX; r < sX + additionalObstaclesR; r++) {
            char[] replaceTemp = template[r].ToCharArray();
            for (int c = sY; c < sY + additionalObstaclesC; c++) {
                replaceTemp[c] = replaceTemplate[(r - sX) * additionalObstaclesC + (c - sY)];
            }
            template[r] = new string(replaceTemp);
        }
    }
    
    private void MakeLadder(in string[] input, in Vector2 position, in GameObject[, ] objs) {
        bool[,] visit = new bool[input.Length, input[0].Length];
        for (int i = 0; i < input.Length; i++) {
            for (int j = 0; j < input[0].Length; j++) {
                if (!visit[i, j] && (input[i][j] == 'H' || input[i][j] == 'h')) {
                    float X = objs[i, j].transform.position.x;
                    float MinY = objs[i, j].transform.position.y;
                    float MaxY = 0f;
                    int count = 0;
                    int k = i;
                    while (k < input.Length && (input[k][j] == 'H' || input[k][j] == 'h')) {
                        MaxY = objs[k, j].transform.position.y;
                        k++;
                        count++;
                    }
                    GameObject LadderParent = CreateLadderParent(new Vector2(X, (MinY + MaxY) * 0.5f));
                    k = i;
                    while (k < input.Length && (input[k][j] == 'H' || input[k][j] == 'h')) {
                        visit[k, j] = true;
                        objs[k, j].transform.parent = LadderParent.transform;
                        k++;
                    }
                    BoxCollider2D collider = LadderParent.GetComponent<BoxCollider2D>();
                    collider.isTrigger = true;
                    collider.size = new Vector2(0.5f, count * 1f);
                }
            }
        }
    }
}
