//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SelectionSpawner : MonoBehaviour
//{
//    ObjectPooler objectPooler;

//    // Serialize Fields
//    [SerializeField] private Vector2 startingPoint; // Initial bottomLeftPoint
//    [SerializeField] private float horizontalGap; // Horizontal gap between Selection Prefabs
//    [SerializeField] private float verticalGap; // Vertical gap between Selection Prefabs
//    //[SerializeField] private Collider2D overlapChecker;

//    // Private Variables
//    private readonly int width = 15; // Minimum number of Selection Prefabs in a row
//    private int height = 15; // Minimum number of Selection Prefabs in a column
//    private string _tag = "Selection";
//    private int isMid = 1; // 1 is True, -1 is False. If True 
//    private RaycastHit2D[] overlapRay; // Checks overlap of selection
//    private int overlapRayLength;

//    private void Start()
//    {a
//        objectPooler = ObjectPooler.Instance; // For Code Simplicity
//        overlapRay = new RaycastHit2D[2];
//        overlapRayLength = overlapRay.Length;

//        #region Initial Spawn

//        Vector2 spawnPosition = startingPoint;
//        for(int row = 0; row < height; ++row)
//        {
//            if(row%2 == 1)
//            {
//                spawnPosition.x = startingPoint.x + (horizontalGap/2);
//            }
//            else
//            {
//                spawnPosition.x = startingPoint.x;
//            }

//            for(int col = 0; col < width; ++col)
//            {
//                objectPooler.SpawnFromPool(_tag, spawnPosition, Quaternion.identity);
//                spawnPosition.x += horizontalGap;
//            }
//            spawnPosition.y += verticalGap;
//        }

//        #endregion

//    }

//    /// <summary>
//    /// private function for spawning selection
//    /// </summary>
//    /// <param name="position">position to start spawning</param>
//    /// <param name="direction">the horizontal/vertical gap</param>
//    /// <param name="size">number of selections to spawn</param>
//    /// <param name="isUpSpawn">True if SpawnUp</param>
//    /// <param name="ignoreFirst">Only for left, right spawn. Spawns obj on even positions</param>
//    private void SpawnSelection(Vector2 position, float direction, int size, bool isUpSpawn, bool ignoreFirst)
//    {
//        if (isUpSpawn)
//        {
//            for (int i = 0; i < size; ++i)
//            {
//                #region Check for overlap of Selection

//                bool isOverlap = false;
//                for(int overlapIndex = 0; overlapIndex < overlapRayLength; ++overlapIndex)
//                {
//                    Vector2 rayPosition = position;
//                    rayPosition.x -= verticalGap * 0.5f * overlapIndex;
//                    overlapRay[overlapIndex] = Physics2D.Raycast(position, Vector2.up, 1f);
//                    if(overlapRay[overlapIndex].collider != null)
//                    {
//                        isOverlap = true;
//                        break;
//                    }
//                }

//                #endregion

//                if (!isOverlap)
//                {
//                    objectPooler.SpawnFromPool(_tag, position, Quaternion.identity);
//                }
//                position.x += direction;
//            }
//        }
//        else
//        {
//            direction *= 2;
//            if(ignoreFirst)
//            {
//                for(int i = 1; i < size; i+=2)
//                {
//                    position.y += direction;

//                    #region Check for overlap of Selection

//                    bool isOverlap = false;
//                    for (int overlapIndex = 0; overlapIndex < overlapRayLength; ++overlapIndex)
//                    {
//                        Vector2 rayPosition = position;
//                        rayPosition.x -= verticalGap * 0.5f * overlapIndex;
//                        overlapRay[overlapIndex] = Physics2D.Raycast(position, Vector2.up, 1f);
//                        if (overlapRay[overlapIndex].collider != null)
//                        {
//                            isOverlap = true;
//                            break;
//                        }
//                    }

//                    #endregion

//                    if (!isOverlap)
//                    {
//                        objectPooler.SpawnFromPool(_tag, position, Quaternion.identity);
//                    }   
//                }
//            }
//            else
//            {
//                for(int i = 0; i < size; i+=2)
//                {
//                    #region Check for overlap of Selection

//                    bool isOverlap = false;
//                    for (int overlapIndex = 0; overlapIndex < overlapRayLength; ++overlapIndex)
//                    {
//                        Vector2 rayPosition = position;
//                        rayPosition.x -= verticalGap * 0.5f * overlapIndex;
//                        overlapRay[overlapIndex] = Physics2D.Raycast(position, Vector2.up, 1f);
//                        if (overlapRay[overlapIndex].collider != null)
//                        {
//                            isOverlap = true;
//                            break;
//                        }
//                    }

//                    #endregion

//                    if (!isOverlap)
//                    {
//                        objectPooler.SpawnFromPool(_tag, position, Quaternion.identity);
//                    }
//                    position.y += direction;
//                }
//            }
//        }
//    }

//    /// <summary>
//    /// When Player moves left or right, spawn selection
//    /// </summary>
//    /// <param name="position">current position of player(before movement)</param>
//    /// <param name="isRight">-1 if left, 1 if right</param>
//    public void SpawnLeftOrRight(Vector2 position, int isRight)
//    {
//        position.x += horizontalGap * 2 * isRight; // 2 is floor(width/2)
//        position.y = (int)(position.y / verticalGap + 1) * verticalGap; // ceil y position
//        if (isMid > 0)
//        {
//            SpawnSelection(position, verticalGap, height, false, false);
//        }
//        else
//        {
//            SpawnSelection(position, verticalGap, height, false, true);
//        }

//        //isMid *= -1;
//    }

//    public void SpawnUp(Vector2 position)
//    {
//        position.x -= horizontalGap * ((width >> 1) + 1);
//        position.y = (int)(position.y / verticalGap + 1) * verticalGap;
//        position.y += 2  * verticalGap;
//        SpawnSelection(position, horizontalGap, width, true, false);
//        isMid *= -1;
//    }
//}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectionSpawner : MonoBehaviour
{
    ObjectPooler objectPooler;

    // Serialize Fields
    [SerializeField] private Vector2 spawnStartPosition;
    [SerializeField] private float horizontalGap;
    [SerializeField] private float verticalGap;

    // Private Variables
    private readonly int width = 15;
    private readonly int height = 15;
    private readonly int halfWidth = 8;
    private readonly int halfHeight = 8;
    private string _tag = "Selection";
    private Dictionary<Vector2, bool> spawnedPosition = new Dictionary<Vector2, bool>();

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;

        Vector2 spawnPosition = spawnStartPosition;
        spawnPosition.x -= horizontalGap * (width / 2);

        #region Initial Spawn

        for (int row = 0; row < height; ++row)
        {
            for (int col = 0; col < width; ++col)
            {
                objectPooler.SpawnFromPool(_tag, spawnPosition, Quaternion.identity);
                spawnPosition.x += horizontalGap;
            }
            spawnPosition.x -= horizontalGap * width;
            spawnPosition.y += verticalGap;
        }

        #endregion

    }

    /// <summary>
    /// private function for spawning selection
    /// </summary>
    /// <param name="position">Start position</param>
    /// <param name="isUpSpawn">true for SpawnUp()</param>
    /// <param name="number">numbers of 'selection' to spawn</param>
    private void SpawnSelection(Vector2 position, bool isUpSpawn, int number)
    {
        spawnedPosition = spawnedPosition.Where(pair => pair.Key.y >= position.y).ToDictionary(pair => pair.Key, pair => pair.Value);
        if (isUpSpawn)
        {
            for(int row = 0; row < 4; ++row)
            {
                for (int col = 0; col < number; ++col)
                {
                    if (!spawnedPosition.ContainsKey(position))
                    {
                        spawnedPosition.Add(position, true);
                        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.up, 2f);
                        if (hit.collider == null)
                        {
                            objectPooler.SpawnFromPool(_tag, position, Quaternion.identity);
                        }
                    }
                    position.x += horizontalGap;
                }
                position.y += verticalGap;
                position.x -= horizontalGap * number;
            }
        }
        else
        {
            // for (int row = 0; row < number; ++row)
            // {
            //     if (spawnedPosition.ContainsKey(position)) continue;
            //     spawnedPosition.Add(position, true);
            //     RaycastHit2D hit = Physics2D.Raycast(position, Vector2.up, 2f);
            //     if (hit.collider == null)
            //     {
            //         objectPooler.SpawnFromPool(_tag, position, Quaternion.identity);
            //     }
            //     position.y += verticalGap;
            // }
        }
    }

    /// <summary>
    /// Spawn Selection at Left or Right
    /// </summary>
    /// <param name="position">Current position of player</param>
    /// <param name="isRight">1 for right spawn, -1 for left spawn</param>
    public void SpawnLeftOrRight(Vector2 position, int isRight)
    {
        position.x += horizontalGap * 4 * isRight;
        position.y = (int)(position.y / verticalGap + 1) * verticalGap;
        SpawnSelection(position, false, height);
    }

    public void SpawnUp(Vector2 position)
    {
        position.x -= horizontalGap * 5;
        position.y = (int)(position.y / verticalGap + 1) * verticalGap;
        //position.y += verticalGap * 4;
        SpawnSelection(position, true, width);
    }
}
