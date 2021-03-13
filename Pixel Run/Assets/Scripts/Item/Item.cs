using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [System.Serializable] private class KeyValue {
        public int Key;
        public int Value;
        public KeyValue(int k, int v) {
            Key = k;
            Value = v;
        }
    }
    [SerializeField] private List<KeyValue> stat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            PlayerStat player = other.gameObject.GetComponent<PlayerStat>();
            for (int i = 0; i < stat.Count; i++) {
                player.UpdateStat(stat[i].Key, stat[i].Value);
            }
            gameObject.SetActive(false);
        }
    }
}
