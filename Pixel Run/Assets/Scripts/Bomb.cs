using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IDamaging, IPooledObject
{
    private float delayTime = 3f;
    private int range = 2;
    private string[] undestroyableTags = { "Ladder", "LadderBar", "Bomb", "Wall", "Treasure", "Portal", "Weapon" };
    private Rigidbody2D myBody;
    private CircleCollider2D circleCollider;
    private Transform myParent;
    private PlayerStat playerStat;

    public int damage { get; set; }
    public string ownerTag { get; set; }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        myBody = gameObject.GetComponent<Rigidbody2D>();
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();
    }

    public void OnObjectSpawn()
    {
        StartCoroutine(Delay());
    }

    private IEnumerator Delay() {
        yield return new WaitForSeconds(delayTime);
        Explosion();
    }

    private void Explosion() {
        //Debug.Log("init");
        Func<float, float> convert = delegate(float f) {
            return (float)Math.Floor(f) + 0.5f;
        };
        Func<int, List<Vector2>> initList = delegate(int r) {
            List<Vector2> ret = new List<Vector2>();
            for (float i = -1f; i <= 1f; i += 1f) {
                for (float j = -1f; j <= 1f; j += 1f) {
                    if (i * j != 0f) continue;
                    ret.Add(new Vector2(i, j));
                }
            }
            return ret;
        };
        
        Vector2 nowPos = new Vector2(convert(gameObject.transform.position.x), convert(gameObject.transform.position.y));
        List<Vector2> DIR = initList(range);
        for (int i = 0; i < DIR.Count; i++) {
            Vector2 now = nowPos + DIR[i];
            for (int j = 0; j < DIR.Count; j++) {
                RaycastHit2D[] hits = Physics2D.RaycastAll(now, DIR[j], 0.55f);
                for (int k = 0; k < hits.Length; k++) {
                    //if (hits[k].transform.gameObject.CompareTag("Ladder") || hits[k].transform.gameObject.name.Equals("h_LadderBar") || hits[k].transform.gameObject.name.Equals("Bomb") || hits[k].transform.gameObject.name.Equals("Wall") || hits[k].transform.gameObject.CompareTag("Treasure") || hits[k].transform.gameObject.CompareTag("Portal") || hits[k].transform.gameObject.CompareTag("Weapon")) continue;
                    if(undestroyableTags.Contains(hits[k].transform.gameObject.tag))
                    {
                        continue;
                    }
                    IDamagable damagable = hits[k].transform.gameObject.GetComponent<IDamagable>();
                    if (damagable != null) {
                        damagable.GetDamage(100);
                    } else {
                        hits[k].transform.gameObject.SetActive(false);
                    }
                }
            }
        }
        EnableRagdoll();
        if (myParent == null) {
            myParent = GameObject.Find("Bomb Parent").transform;
        }
        transform.parent = myParent;
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other) {
        if (!playerStat.adhesive || myBody.velocity == Vector2.zero) {
            return;
        }
        if (other.gameObject.layer == 9) {
            return;
        }
        if (IsDetect(other.gameObject)) {
            myBody.velocity = Vector2.zero;
            DisableRagdoll();
            transform.parent = other.gameObject.transform;
        }
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other) {
        if (!playerStat.adhesive || myBody.velocity == Vector2.zero) {
            return;
        }
        if (other.gameObject.layer == 9) {
            return;
        }
        if (IsDetect(other.gameObject)) {
            myBody.velocity = Vector2.zero;
            DisableRagdoll();
            transform.parent = other.gameObject.transform;
        }
    }

    private bool IsDetect(in GameObject other) {
        if (other.gameObject != null) {
            if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Environment") || other.gameObject.CompareTag("Wall")) {
                return true;
            }
        }
        return false;
    }

    void DisableRagdoll() {
        myBody.isKinematic = true;
        circleCollider.enabled = false;
    }

    void EnableRagdoll() {
        myBody.isKinematic = false;
        circleCollider.enabled = true;
    }
}
