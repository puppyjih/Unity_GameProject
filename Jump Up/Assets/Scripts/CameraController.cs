using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    Transform AT;
    //private Vector2 offset;

    // Start is called before the first frame update
    void Start()
    {
        AT = player.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(0, AT.position.y,-1);
    }
}
