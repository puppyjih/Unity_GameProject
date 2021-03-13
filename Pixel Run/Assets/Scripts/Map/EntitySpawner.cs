using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EntitySpawner : MonoBehaviour
{
    //[System.Serializable]
    //private class ForMappingInt : ForMapping<int> {
    //    public ForMappingInt(int __Key__, GameObject __Value__) : base(__Key__, __Value__) {}
    //}


    //[SerializeField] private ForMappingInt[] forEnemy;
    //private Dictionary<int, GameObject> objectEnemy = { };
    [SerializeField] private GameObject[] objectEnemy;
    [SerializeField] private GameObject[] objectEntity;
    [SerializeField] private Transform mapParent;

    private readonly int row = 9;
    private readonly int col = 12;
    //private readonly char[] solidTiles = { '1' };
    private readonly char[] unSolidTiles = { '0', 'H', 'h' };
    private int[, ] isCollision;
    private Vector2Int[, ] consecutiveL, consecutiveR; // L, R ==> consecutive collision (x: type, y: count)
    private int startX = 0;
    private int startY = 0;
    private int endX = 0;
    private int endY = 0;


    /// <summary>
    /// Spawns Enemies for Stage 1
    /// </summary>
    /// <param name="map">Map template string</param>
    /// <param name="roomX">Current Room's x position(0~3)</param>
    /// <param name="roomY">Current Room's y position(0~3)</param>
    /// <param name="position">World position to start spawning enemies</param>
    public void SpawnEnemyStage1(string[] map, int roomX, int roomY, Vector2 position)
    {
        SetCollision(map);

        SetConsecutive(map);
        if(roomX != startX || roomY != startY) // Don't spawn enemies on starting room
        {
            for (int r = 0; r < row; ++r)
            {
                for (int c = 0; c < col; ++c)
                {
                    //Spawn Moth
                    if (isCollision[r, c] == 0)
                    {
                        if (r > 1 && isCollision[r - 1, c] == 1 && (c > 0 && isCollision[r, c - 1] == 0 || c + 1 < col && isCollision[r, c + 1] == 0))
                        {
                            if (Random.Range(0, 45) == 0)
                            {
                                SpawnEnemy(0, position, r, c, 2);
                            }
                        }
                    }

                    //Spawn Mouse
                    if (isCollision[r, c] == 0) 
                    {
                        // if (r + 1 < row && isCollision[r + 1, c] == 1)
                        if (r + 1 < row && isCollision[r + 1, c] == 1 && GetConsecutive(consecutiveL, r, r + 1, c) + GetConsecutive(consecutiveR, r, r + 1, c) - 2 >= 3)
                        {
                            if (Random.Range(0, 45) == 0)
                            {
                                SpawnEnemy(1, position, r, c, 2);
                            }
                        }
                    }

                    //Spawn Mole
                    if (isCollision[r, c] == 1 && isCollision[r, c] != 2)
                    {
                        if (r > 1 && isCollision[r - 1, c] == 0 && GetConsecutive(consecutiveL, r, r - 1, c) + GetConsecutive(consecutiveR, r, r - 1, c) - 2 >= 3)
                        {
                            if (Random.Range(0, 45) == 0)
                            {
                                SpawnEnemy(2, position, r, c, 2);
                            }
                        }
                    }

                    if (isCollision[r, c] == 0 && r + 1 < row && isCollision[r + 1, c] == 1) {
                        int[] d = new int[6]{0, 1, 0, -1, -1, 0};
                        int cnt = 0;
                        for (int i = 0; i + 1 < 6; i += 2) {
                            int nr = r + d[i],
                                nc = c + d[i + 1];
                            if (nr < 0 || row <= nr || nc < 0 || col <= nc) continue;
                            if (isCollision[nr, nc] == 1) cnt++;
                        }
                        if (cnt >= 2) {
                            if (Random.Range(0, 40) == 0) {
                                SpawnEntity(0, position, r, c, 3);
                            }
                        }

                        if (isCollision[r, c] == 0 && r + 1 < row && isCollision[r + 1, c] == 1) {
                            if (Random.Range(0, 50) == 0) {
                                SpawnEntity(1, position, r, c, 3);
                            }
                        }
                    }
                    position.x += 1f;
                }
                position.y -= 1f;
                position.x -= col * 1f;
            }
        }
    }

    private void SpawnEnemy(int number, Vector2 position, int r, int c, int collisionType) {
        GameObject obj = Instantiate(objectEnemy[number], position, Quaternion.identity);
        obj.name = objectEnemy[number].name;
        obj.transform.parent = mapParent;
        isCollision[r, c] = collisionType;
    }

    private void SpawnEntity(int number, Vector2 position, int r, int c, int collisionType) {
        GameObject obj = Instantiate(objectEntity[number], position, Quaternion.identity);
        obj.name = objectEntity[number].name;
        obj.transform.parent = mapParent;
        isCollision[r, c] = collisionType;
    }

    private void SetCollision(string[] map)
    {
        isCollision = new int[row, col];
        for(int r = 0; r < row; ++r)
        {
            for(int c = 0; c < col; ++c)
            {
                if(unSolidTiles.Contains(map[r][c]))
                {
                    isCollision[r, c] = 0;
                }
                else
                {
                    isCollision[r, c] = 1;
                }
            }
        }
    }

    public void SetStartPosition(int x, int y)
    {
        startX = x;
        startY = y;
    }

    public void SetEndPosition(int x, int y)
    {
        endX = x;
        endY = y;
    }

    private int GetCollisionCount(int i, int j) {
        return consecutiveL[i, j].y + consecutiveR[i, j].y - 1;
    }

    private int GetConsecutive(in Vector2Int[, ] target, int i, int compareI, int j) {
        return Utility.Min(target[i, j].y, target[compareI, j].y);
    }

    private void SetConsecutive(in string[] map) {
        consecutiveL = new Vector2Int[row, col];
        consecutiveR = new Vector2Int[row, col];
        for (int i = 0; i < row; i++) {
            for (int j = 0; j < col; j++) {
                if (j == 0) {
                    consecutiveL[i, j].x = isCollision[i, j];
                    consecutiveL[i, j].y = 1;
                } else {
                    int add = 0;
                    if (consecutiveL[i, j - 1].x == isCollision[i, j]) {
                        add = consecutiveL[i, j - 1].y;
                    }
                    consecutiveL[i, j].x = isCollision[i, j];
                    consecutiveL[i, j].y = add + 1;
                }
            }
            for (int j = col - 1; j >= 0; j--) {
                if (j + 1 == col) {
                    consecutiveR[i, j].x = isCollision[i, j];
                    consecutiveR[i, j].y = 1;
                } else {
                    int add = 0;
                    if (consecutiveR[i, j + 1].x == isCollision[i, j]) {
                        add = consecutiveR[i, j + 1].y;
                    }
                    consecutiveR[i, j].x = isCollision[i, j];
                    consecutiveR[i, j].y = add + 1;
                }
            }
        }
    }

    public void SpawnPlayer(string[] map, Vector2 position) {
        Vector2Int p = FindPositionCreateObject(map);
        GameObject.Find("Player").transform.position = position + new Vector2(p.y * 1f, -p.x * 1f);
    }

    public void SpawnPortal(string[] map, Vector2 position) {
        Vector2Int p = FindPositionCreateObject(map);
        GameObject.Find("Portal").transform.position = position + new Vector2(p.y * 1f, -p.x * 1f);
    }

    public void SpawnSpecialPortal(string[] map, Vector2 position) {
        Vector2Int p = FindPositionCreateObject(map);
        GameObject.Find("GoldenPortal").transform.position = position + new Vector2(p.y * 1f, -p.x * 1f);
    }

    private Vector2Int FindPositionCreateObject(in string[] template) {
        List<Vector2Int> possibleList = new List<Vector2Int>();
        Vector2Int[] DXY = new Vector2Int[3]{new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1)};
        for (int j = 0; j < template[0].Length; j++) {
            for (int i = 0; i + 1 < template.Length; i++) {
                if (template[i][j] != '0' || template[i + 1][j] != '1') continue;
                int c = 0;
                for (int k = 0; k < 3; k++) {
                    int nx = i + DXY[k].x, ny = j + DXY[k].y;
                    if (nx < 0 || template.Length <= nx || ny < 0 || template[0].Length <= ny) continue;
                    c += template[nx][ny] == '0' ? 1 : 0;
                }
                if (c == 0) continue;
                possibleList.Add(new Vector2Int(i, j));
            }
        }
        return Utility.GetRandom<Vector2Int>(possibleList);
    }
}
