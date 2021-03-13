using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalTrap : MonoBehaviour
{
    [SerializeField] private HorizontalTrapInnerObject horizontalTrapInnerObject;
    private RaycastHit2D hitU, hitD; // Up, Down
    private Vector3 position;
    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector3(transform.localScale.x, 0f, 0f);
        position = transform.position + direction * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        bool isHitSuccess(RaycastHit2D hit) {
            if (hit.collider == null) return false;
            if (hit.transform.gameObject.GetComponent<HorizontalTrapInnerObject>() != null) return false;
            return true;
        }
        Rigidbody2D rbU = null, rbD = null;
        hitU = Physics2D.Raycast(position + new Vector3(0f, 0.3f, 0f), direction, 6f);
        hitD = Physics2D.Raycast(position - new Vector3(0f, 0.3f, 0f), direction, 6f);
        if (isHitSuccess(hitU)) rbU = hitU.transform.gameObject.GetComponent<Rigidbody2D>();
        if (isHitSuccess(hitD)) rbD = hitD.transform.gameObject.GetComponent<Rigidbody2D>();
        Debug.DrawRay(position + new Vector3(0f, 0.3f, 0f), direction * 6f, Color.green);
        Debug.DrawRay(position - new Vector3(0f, 0.3f, 0f), direction * 6f, Color.green);
        if (rbU != null || rbD != null) {
            horizontalTrapInnerObject.ActionStart();
        }
    }
}
