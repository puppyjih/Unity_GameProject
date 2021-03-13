using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangOn : MonoBehaviour
{
    private Rigidbody2D myBody;
    private Player player;
    private bool __isHangOn = false;
    public bool isHangOn { get { return __isHangOn; } set { __isHangOn = value; } }
    private float jumpForce;

    void Start()
    {
        myBody = gameObject.GetComponent<Rigidbody2D>();
        player = gameObject.GetComponent<Player>();
        jumpForce = player.getJumpForce();
    }

        /// <summery>
    /// Hang On Brick & if it's success then return true not return false;
    /// </summery>
    public void Enter(float dir) 
    {
        if (myBody.velocity == Vector2.zero)
        {
            return;
        }
        if (myBody.velocity.x == 0f)
        {
            return;
        }
        Vector2 Clamp(Vector2 v) 
        {
            Func<float, float> c = delegate(float f) 
            {
                return (int)f + 0.5f;
            };
            return new Vector2(c(v.x), c(v.y));
        }
        float Abs(float f) 
        {
            return f > 0f ? f : -f;
        }
        bool Check(Vector2 start, Vector2 direction, float distance) // return crash thing (like brick)
        { 
            RaycastHit2D[] hits = Physics2D.RaycastAll(start, direction, distance);
            if (hits.Length == 0)
            {
                return false;
            }
            int count = 0;
            for (int i = 0; i < hits.Length; i++) 
            {
                if (hits[i].transform.gameObject.GetComponent<Rigidbody2D>() == null && !hits[i].transform.gameObject.CompareTag("LadderBar")) count++;
            }
            return count > 0;
        }
        Vector2 position = Clamp(transform.position) + dir * new Vector2(0.4f, 0);
        if (Abs(transform.position.x - position.x) > 0.3f || Abs(transform.position.y - position.y) > 0.1f)
        {
            return;
        }
        if (Check(position + Vector2.up * 0.4f, new Vector2(dir, 0), 0.15f) && !Check(position + Vector2.up, new Vector2(dir, 0), 0.15f) && !Check(position + Vector2.down * 0.3f, Vector2.down, 0.3f)) 
        {
            myBody.bodyType = RigidbodyType2D.Kinematic;
            myBody.velocity = Vector2.zero;
            transform.position = position - dir * new Vector2(0.25f, 0) + 0.2f * Vector2.up;
            isHangOn = true;
            return;
        }
    }

    /// <summery>
    /// only input jump key ==> go up, jump key and down key ===> go down
    /// </summery>
    /// <param name="vertial">verical is have -1 or 1, -1 is down, 1 is up</parm>
    public void Exit(float vertical) 
    {
        myBody.bodyType = RigidbodyType2D.Dynamic;
        if (vertical > 0f)
        {
            
            myBody.velocity = new Vector2(0f, jumpForce); // because fixed position
        }
        StartCoroutine(Wait());
    }

    IEnumerator Wait() {
        yield return new WaitForSeconds(0.1f);
        isHangOn = false;
    }
}