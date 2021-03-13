using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefaultWeapon : MonoBehaviour, IWeapon, IDamaging {
    [SerializeField] private int __damage__;
    [SerializeField] private float __attackSpeed__ = 1f;
    [SerializeField] private float __spriteAngle__;
    [SerializeField] private string __ownerTag__;
    private BoxCollider2D boxCollider2D;
    private GameObject image;

    public int damage { get { return __damage__; } set { __damage__ = value; } }
    public float attackSpeed { get { return __attackSpeed__; } set { __attackSpeed__ = value; } }
    public float spriteAngle { get { return __spriteAngle__; } set { __spriteAngle__ = value; } }
    public string ownerTag { get { return __ownerTag__; } set { __ownerTag__ = value; } }
    private void Start() {
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        image = transform.GetChild(0).gameObject;
    }

    public void Attack() {
        StartCoroutine(ImageActive());
    }

    IEnumerator ImageActive() {
        image.SetActive(true);
        boxCollider2D.enabled = true;
        yield return new WaitForSeconds(attackSpeed);
        boxCollider2D.enabled = false;
        image.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
        if (damagable != null) {
            damagable.GetDamage(damage);
        }
    }
}