using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Probabilistic : MonoBehaviour
{
    [SerializeField] private GameObject noneProbabilityObject;
    [SerializeField] private float __probability__;
    public float probability { get { return __probability__; } }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetObject() {
        if (probability >= Random.Range(0, 100001) * 0.001) {
            return this.gameObject;
        } else {
            return noneProbabilityObject;
        }
    }
}
