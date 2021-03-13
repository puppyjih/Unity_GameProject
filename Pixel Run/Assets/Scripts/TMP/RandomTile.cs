using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomTile {
    [System.Serializable]
    public class ItemClass {
        public float probability;
        public GameObject item;
        public ItemClass(float p, GameObject i) {
            probability = p;
            item = i;
        }
    }
    public List<ItemClass> items;
    public RandomTile() {
        items = new List<ItemClass>();
    }

    public GameObject GetObject() {
        float myProbability = Random.Range(0, 100001) * 0.001f;
        float nowProbability = 0f;
        for (int i = 0; i < items.Count; i++) {
            nowProbability += items[i].probability;
            if (nowProbability >= myProbability) {
                return items[i].item;
            }
        }
        return null;
    }
}