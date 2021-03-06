using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// -Tㅗ+
public class MapGenerator : MonoBehaviour
{
    private class Line {
        public bool isHorizontal { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public Line(float __X__, float __Y__, bool __isHorizontal__ = true) {
            X = __X__;
            Y = __Y__;
            isHorizontal = __isHorizontal__;
        }
    }
    // private variable
    //////////////////////////////////////////////// REVIEW come from check because fill 0
    private List<Vector2Int> fromUp = new List<Vector2Int>{new Vector2Int(0, 90), new Vector2Int(3, 5), new Vector2Int(4, 5)};
    private List<Vector2Int> fromRight = new List<Vector2Int>{new Vector2Int(0, 60), new Vector2Int(3, 30), new Vector2Int(3, 5), new Vector2Int(3, 5)};
    private List<Vector2Int> fromDown = new List<Vector2Int>{new Vector2Int(0, 70), new Vector2Int(1, 20), new Vector2Int(2, 5), new Vector2Int(4, 5)};
    private List<Vector2Int> fromLeft = new List<Vector2Int>{new Vector2Int(0, 50), new Vector2Int(1, 20), new Vector2Int(2, 20), new Vector2Int(3, 5), new Vector2Int(4, 5)};
    /// <summery>
    /// <param name="List Vector2int">x => value, y => probability</param>
    private int getItemFromList(in List<Vector2Int> l) {
        int probability = UnityEngine.Random.Range(0, 1000001) % 101;
        int myProbability = 0;
        for (int i = 0; i < l.Count; i++) {
            myProbability += l[i].y;
            if (myProbability >= probability) {
                return l[i].x;
            }
        }
        return l[0].x;
    }
    private Vector2Int[] DIR = new Vector2Int[4]{new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1)};
    ////////////////////////////////////////////////
    private TemplateGenerator templateGenerator;
    private EntitySpawner entitySpawner;
    private string fileName = "0.txt";
    private float spaceX = 12f, spaceY = 9f;
    Vector2 pos = new Vector2(1.5f, 0.5f);
    private Transform mapParent;
    //
    private int[,] gameMap;
    private List<Vector2Int> solutionPath;
    private int mapR, mapC;
    private GameObject Wall;
    // public variable
    public Vector2Int startPoint;

    private void Awake()
    {
        mapParent = GameObject.Find("MapParent").transform;
        templateGenerator = gameObject.GetComponent<TemplateGenerator>();
        entitySpawner = gameObject.GetComponent<EntitySpawner>();
        Wall = GameObject.Find("Wall");
    }

    void MakeWall() {
        Line[] line = new Line[4]{new Line(-1, 0), new Line(-2, mapR * spaceY + 1), new Line(-1, 0, false), new Line(mapC * spaceX, 1, false)};
        GameObject obj;
        for (int l = 0; l < 4; l++) {
            if (line[l].isHorizontal) {
                for (float i = 1f; i <= mapC * spaceX + 1f; i += 1f) {
                    obj = Instantiate(Wall, pos + new Vector2(i + line[l].X, line[l].Y), Quaternion.identity);
                    obj.name = "Wall";
                    obj.transform.parent = mapParent;
                }
            } else {
                for (float i = 0f; i < mapR * spaceY + 1f; i += 1f) {
                    obj = Instantiate(Wall, pos + new Vector2(line[l].X, i + line[l].Y), Quaternion.identity);
                    obj.name = "Wall";
                    obj.transform.parent = mapParent;
                }
            }
        }
    }

    public void MapSpawn(string dirPath, int R, int C, bool isExistSpecial) {
        mapR = R;
        mapC = C;
        SolutionGeneration();
        string[] template;
        bool doSpecialPortalSpawn = false;
        int specialGate = mapR * mapC - 2;
        for (int i = 0; i < mapR; i++) {
            for (int j = 0; j < mapC; j++) {
                Vector2Int typeCheck = new Vector2Int(i, j);
                Vector2 spawnPosition = pos + new Vector2(spaceX * j, mapR * spaceY - spaceY * i);
                int type = 0;
                specialGate--;
                if (typeCheck == solutionPath[0]) {
                    specialGate++;
                    type = 1;
                    entitySpawner.SetStartPosition(i, j);
                } else if (typeCheck == solutionPath[solutionPath.Count - 1]) {
                    specialGate++;
                    type = 2;
                    entitySpawner.SetEndPosition(i, j);
                }
                if (isExistSpecial && (specialGate == 0 || Utility.GetRandomRange(0, specialGate) == 0)) {
                    doSpecialPortalSpawn = true;
                }
                templateGenerator.Spawn(dirPath + gameMap[i, j], spawnPosition, type, out template);
                if (type != 1) entitySpawner.SpawnEnemyStage1(template, i, j, spawnPosition);
                if (type == 1) entitySpawner.SpawnPlayer(template, spawnPosition);
                else if (type == 2) entitySpawner.SpawnPortal(template, spawnPosition);
                if (doSpecialPortalSpawn) {
                    entitySpawner.SpawnSpecialPortal(template, spawnPosition);
                    doSpecialPortalSpawn = false;
                }
            }
        }
        MakeWall();
    }

    public void SpecialStage(string dirPath) {
        string filePath = dirPath + "MapType";
        TextAsset[] files = Resources.LoadAll<TextAsset>(filePath);
        TextAsset file = files[UnityEngine.Random.Range(0, files.Length)];
        string[] map = file.text.Split('\n');
        string[] template;
        int type = 0;
        mapR = map.Length;
        mapC = map[0].Length;
        for (int i = 0; i < map.Length; i++) {
            for (int j = 0; j < map[0].Length; j++) {
                type = 0;
                if (i == 0 && j == 0) type = 1;
                else if (i == map.Length - 1) {
                    if (i % 2 == 0 && j == map[0].Length - 1) {
                        type = 2;
                    } else if (i % 2 == 1 && j == 0) {
                        type = 2;
                    }
                }
                Vector2 spawnPosition = pos + new Vector2(spaceX * j, mapR * spaceY - spaceY * i);
                templateGenerator.Spawn(dirPath + map[i][j].ToString(), spawnPosition, type, out template);
                if (type == 1) entitySpawner.SpawnPlayer(template, spawnPosition);
                else if (type == 2) entitySpawner.SpawnPortal(template, spawnPosition);
            }
        }
        MakeWall();
    }

    private void SolutionGeneration() {
        gameMap = new int[mapR, mapC];
        solutionPath = new List<Vector2Int>();
        int curR = 0;
        int curC = UnityEngine.Random.Range(0, mapC);
        startPoint = new Vector2Int(curR, curC);
        while (curR < mapR) {
            gameMap[curR, curC] = 1;
            solutionPath.Add(new Vector2Int(curR, curC));
            int c = UnityEngine.Random.Range(0, 5);
            // find proper position
            if (c < 2 && curC > 0 && gameMap[curR, curC - 1] != 1) curC--;
            else if (2 <= c && c < 4 && curC < mapC - 1 && gameMap[curR, curC + 1] != 1) curC++; 
            else curR++;
        }
        // Type: 1 => -, 2 => T, 3 => ㅗ, 4 => +
        Func<Vector2Int, int> GetR = delegate(Vector2Int p) {
            return p.x;
        };
        Func<Vector2Int, int> GetC = delegate(Vector2Int p) {
            return p.y;
        };
        Func<int[], int> myRandomArray = delegate(int[] a) {
            return a[UnityEngine.Random.Range(0, a.Length)];
        };
        // First
        Vector2Int prev, cur, next;
        cur = solutionPath[0];
        next = solutionPath[1];
        if (GetR(cur) == GetR(next)) { // -
            gameMap[GetR(cur), GetC(cur)] = 1;
        } else { // |
            gameMap[GetR(cur), GetC(cur)] = 2;
        }
        int solutionPathLength = solutionPath.Count;
        for (int i = 1; i < solutionPathLength - 1; i++) {
            prev = solutionPath[i - 1];
            cur = solutionPath[i];
            next = solutionPath[i + 1];
            if (GetR(prev) == GetR(cur)) {
                if (GetR(cur) == GetR(next)) { // --
                    gameMap[GetR(cur), GetC(cur)] = 1;
                } else { // -|
                    gameMap[GetR(cur), GetC(cur)] = 2;
                }
            } else {
                if (GetR(cur) == GetR(next)) { // |-
                    gameMap[GetR(cur), GetC(cur)] = 3;
                } else { // ||
                    gameMap[GetR(cur), GetC(cur)] = 4;
                }
            }
        }
        prev = solutionPath[solutionPathLength - 2];
        cur = solutionPath[solutionPathLength - 1];
        if (GetR(prev) == GetR(cur)) { // -
            gameMap[GetR(cur), GetC(cur)] = 1;
        } else { // |
            gameMap[GetR(cur), GetC(cur)] = 3;
        }

        // Fill Empty Space by DFS Instead using 0 template
        for (int i = 0; i < mapR; i++) {
            for (int j = 0; j < mapC; j++) {
                if (gameMap[i, j] == 0) {
                    bool[, ] visit = new bool[mapR, mapC];
                    fillByDFS(new Vector2Int(i, j), ref visit);
                }
            }
        }


        // DEBUGMAP(false);
    }

    void DEBUGMAP(bool option = true) {
        string s = "";
        string shape = "-Tㅗ+";
        for(int i = 0; i < mapR; i++) {
            for(int j = 0; j < mapC; j++) {
                if (gameMap[i, j] == 0 || option)
                    s += gameMap[i, j].ToString();
                else
                    s += shape[gameMap[i, j] - 1].ToString();
            }
            s += "\n";
        }
        Debug.Log(s);
    }

    private void fillByDFS(Vector2Int now, ref bool[, ] visit) {
        int x = now.x, y = now.y;
        DIR = DIR.OrderBy(d => Guid.NewGuid()).ToArray();
        for (int i = 0; i < 4; i++) {
            int nx = now.x + DIR[i].x, ny = now.y + DIR[i].y;
            if (nx < 0 || mapR <= nx || ny < 0 || mapC <= ny || visit[nx, ny]) continue;
            visit[nx, ny] = true;
            fillByDFS(now + DIR[i], ref visit);
            if (gameMap[x, y] != 0) continue;
            if (gameMap[nx, ny] != 0) {
                int item = -1;
                if (nx - x > 0) { // come down
                    item = getItemFromList(fromDown);
                } else if (nx - x < 0) { // come up
                    item = getItemFromList(fromUp);
                } else if (ny - y > 0) { // come right
                    item = getItemFromList(fromRight);
                } else { // from left
                    item = getItemFromList(fromLeft);
                }
                gameMap[x, y] = item;
                return;
            }
        }
    }
}