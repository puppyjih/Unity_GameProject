using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    private Vector2 basicThrowDelta = new Vector2(2f, 1f);
    private float throwSpeed = 10f;
    private Rigidbody2D myBody;
    private Transform _transform;
    private bool throwing = false;
    private int damage = 10;
    void Awake() {
        myBody = gameObject.GetComponent<Rigidbody2D>();
        _transform = gameObject.transform;
    }

    private void FixedUpdate() {
        if (myBody.velocity == Vector2.zero) {
            throwing = false;
        }
    }

    public void Throw(Vector2 playerVelocity, float direction) {
        Func<float, Vector2, float> Bounding = delegate(float x, Vector2 d) {
            if (x < d.x) return d.x;
            if (d.y < x) return d.y;
            return x;
        };
        Func<float, float> Abs = delegate (float x)
        {
            return x > 0f ? x : -x;
        };
        Vector2 throwVector = direction * basicThrowDelta + playerVelocity;
        throwVector.x = direction * Bounding(Abs(throwVector.x), new Vector2(1f, 2.5f));
        throwVector.y = Bounding(throwVector.y, new Vector2(0.6f, 1.5f));
        //_transform.parent = null;
        throwing = true;
        myBody.bodyType = RigidbodyType2D.Dynamic;
        myBody.velocity = throwVector * throwSpeed;
        myBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!throwing)
        {
            return;
        }
        IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
        if (damagable != null && !other.CompareTag("Player")) {
            damagable.GetDamage(damage);
        }
    }
}
