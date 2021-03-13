using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Rope : SkillParent
{
    // Start is called before the first frame update
    [SerializeField] private string ropeTag;

    private GameObject[] tailrope;
    private GameObject newRope;
    private Vector2 spawnPoint;
    private ObjectPooler objectPooler;

    private int layerMask = (1 << 11);

    void Start()
    {
        SetCost(10);
        SetCoolTime(1);
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        tailrope = new GameObject[8];
        objectPooler = ObjectPooler.Instance;
    }

    // Update is called once per frame
    public override void Activate()
    {
        spawnPoint.x = (int)player.transform.position.x + 0.5f;
        spawnPoint.y = (int)player.transform.position.y + 0.5f;
        newRope = objectPooler.SpawnFromPool(ropeTag, spawnPoint, Quaternion.identity);
        for (int i = 1; i <= 8; i++)
        {
            tailrope[i-1] = newRope.transform.GetChild(i).gameObject;
            tailrope[i-1].SetActive(false);
        }
        StartCoroutine(MoveRope());
    }

    IEnumerator MoveRope()
    {
        float timeCounter = 0f;
        float boundX = newRope.GetComponentInChildren<SpriteRenderer>().bounds.extents.x;
        float boundY = newRope.GetComponentInChildren<SpriteRenderer>().bounds.extents.y;

        while (timeCounter <= 0.5f)
        {
            Vector2 upPosition = new Vector2(newRope.transform.position.x, newRope.transform.position.y + boundY - 0.1f);
            RaycastHit2D upRay = Physics2D.Raycast(upPosition, Vector2.up , 0.5f, layerMask);
            if (upRay.collider != null/* && upRay.collider.name != "Player"*/)
            {
                //Debug.Log(upRay.collider.name);
                break;
            }
            newRope.transform.position = new Vector2(newRope.transform.position.x, newRope.transform.position.y + 10f * Time.deltaTime);
            timeCounter += Time.deltaTime;
            yield return null;
        }
        newRope.transform.position = new Vector2((int)newRope.transform.position.x + 0.5f, (int)newRope.transform.position.y + 0.5f);

        int i = 0;
        while(timeCounter <= 0.7f && i <= 7)
        {
            Vector2 downPosition = new Vector2(newRope.transform.position.x, newRope.transform.position.y - boundY + 0.1f - i);
            RaycastHit2D downRay = Physics2D.Raycast(downPosition, Vector2.down, 0.5f, layerMask);
            Debug.DrawRay(downPosition, Vector2.down * 0.5f);
            if (downRay.collider != null/* && downRay.collider.name != "Player"*/)
            {
                //Debug.Log(downRay.collider.name);
                break;
            }
            //newRope.transform.position = new Vector2(newRope.transform.position.x, newRope.transform.position.y + 10f * Time.deltaTime);
            tailrope[i++].SetActive(true);
            timeCounter += Time.deltaTime;

            yield return null;
        }

        Vector2 colliderPos = new Vector2(0 ,(-i)/2f);

        BoxCollider2D boxbox = newRope.gameObject.AddComponent<BoxCollider2D>();
        newRope.tag = "Ladder";
        boxbox.offset = colliderPos;
        boxbox.isTrigger = true;
        boxbox.size = new Vector2(boundX, i + 1);
        yield return null;
    }
}
