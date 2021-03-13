using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        int numOfsprites = sprites.Length;
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, numOfsprites)];
    }
}
