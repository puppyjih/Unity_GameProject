using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalDisabler : MonoBehaviour
{
    //private void Start()
    //{
    //    gameObject.SetActive(false);
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.SetActive(false);
        //StartCoroutine(Rotating(collision.gameObject));
    }

    //public void SetActive(bool active)
    //{
    //    gameObject.SetActive(active);
    //}

    //IEnumerator Rotating(GameObject __obj__) {

    //    CircleCollider2D __obj__collider__ = __obj__.GetComponent<CircleCollider2D>();
    //    __obj__collider__.enabled = false;
    //    while (__obj__.transform.localScale.x > 0f) {
    //        __obj__.transform.localScale = new Vector2(__obj__.transform.localScale.x - 0.2f, __obj__.transform.localScale.y - 0.2f);
    //        yield return new WaitForSeconds(0.0005f);
    //    }
    //    __obj__collider__.enabled = true;
    //    __obj__.SetActive(false);
    //}
}
