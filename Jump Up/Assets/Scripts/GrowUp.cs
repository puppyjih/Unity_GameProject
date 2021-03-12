using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowUp : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 upvec;
    public Vector2 vect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        vect +=  Time.fixedDeltaTime*upvec;
        rb.MovePosition(vect);

    }
}
