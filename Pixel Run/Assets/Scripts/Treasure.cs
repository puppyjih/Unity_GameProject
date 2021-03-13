using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour, IDamagable
{
    [System.Serializable]
    public class ItemInfo {
        public float probability;
        public GameObject OBJ;
        public ItemInfo(float __probability__, GameObject __object__) {
            probability = __probability__;
            OBJ = __object__;
        }
    }
    public float probability { get; set; }
    [SerializeField] private float __probability__;
    [SerializeField] private List<ItemInfo> L;
    // [SerializeField] private ProbabilityList<GameObject> probabilityList;
    
    public int HP { get; set; }
    public string colliderToIgnore { get; set; }
    private bool isDead = false;

    private GameObject GetObject() {
        float myProbability = Random.Range(0, 100001) * 0.001f;
        float nowProbability = 0f;
        for (int i = 0; i < L.Count; i++) {
            nowProbability += L[i].probability;
            if (nowProbability >= myProbability) {
                return L[i].OBJ;
            }
        }
        return null;
    }

    void Awake()
    {
        HP = 10;
        probability = __probability__;
        L = L.OrderByDescending(t => t.probability).ToList();
    }

    private void SpawnItem() {
        GameObject obj = Instantiate(GetObject(), transform.position, Quaternion.identity);
        obj.transform.parent = GameObject.Find("MapParent").transform;
        gameObject.SetActive(false);
    }

    ///// <summary>
    ///// Sent when another object enters a trigger collider attached to this
    ///// object (2D physics only).
    ///// </summary>
    ///// <param name="other">The other Collider2D involved in this collision.</param>
    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        SpawnItem();
    //    }
    //}

    public void GetDamage(int damage)
    {
        HP -= damage;
        if(HP <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        if (isDead)
        {
            return;
        }
        else
        {
            isDead = true;
            SpawnItem();
        }
    }
}
