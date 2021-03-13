using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalTrap : MonoBehaviour
{
    [SerializeField] private VerticalTrapInnerObject verticalTrapInnerObject;
    private RaycastHit2D hitL, hitR; // Up, Down
    private Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        bool isHitSuccess(RaycastHit2D hit) {
            if (hit.collider == null) return false;
            if (hit.transform.gameObject.GetComponent<VerticalTrapInnerObject>() != null) return false;
            return true;
        }
        Rigidbody2D rbL = null, rbR = null;
        hitL = Physics2D.Raycast(transform.position + new Vector3(0.33f, 0.5f, 0f), Vector2.up, 1f);
        hitR = Physics2D.Raycast(transform.position + new Vector3(-0.33f, 0.5f, 0f), Vector2.up, 1f);
        if (isHitSuccess(hitL)) rbL = hitL.transform.gameObject.GetComponent<Rigidbody2D>();
        if (isHitSuccess(hitR)) rbR = hitR.transform.gameObject.GetComponent<Rigidbody2D>();
        Debug.DrawRay(transform.position + new Vector3(0.33f, 0.5f, 0f), Vector2.up * 1f, Color.green);
        Debug.DrawRay(transform.position + new Vector3(-0.33f, 0.5f, 0f), Vector2.up * 1f, Color.green);
        if (rbL != null && rbL.gameObject.CompareTag("Player") || rbR != null && rbR.gameObject.CompareTag("Player")) {
            verticalTrapInnerObject.ActionStart();
        }
    }
}
