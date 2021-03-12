using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MovePlayer : MonoBehaviour
{
    private Rigidbody2D rb;
    public Vector2 velocityX;
    public Vector2 velocityY;
    public Animator animator;
    public float JumpSpeed;
    public bool isFlipped = false;
    public int maxJump = 1;
    public LayerMask c2D;
    public bool isDrown = false;

    public Transform groundCheck;
    public LayerMask groundLayers;
    public float Speed = 7.0f;
    public float jumpForce = 300.0f;
    public int height;
    public Text text;
    public Text winText;

    public bool isGrounded = false;
    private float groundCheckRadius = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //c2D = GetComponent<Collider2D>();
        height = 0;
        text.text = "Height : " + height.ToString();
        winText.text = "";
        groundCheck.position = new Vector2(groundCheck.position.x, groundCheck.position.y-0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        isDrown = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, c2D);

        if (isDrown != true)
        {
            height = (int)rb.position.y;
            text.text = "Height : " + height.ToString();

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Quit();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && maxJump <= 2)
            {
                if (isGrounded == true)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    //rb.AddForce(new Vector2(0, jumpForce));


                    Vector2 jump = new Vector2(0, 3);

                    //rb.AddForce(jump*JumpSpeed);
                    rb.AddForce(jump * JumpSpeed * Time.fixedDeltaTime);
                    animator.SetTrigger("JumpKey");
                    maxJump++;
                    height = (int)rb.position.y;
                    text.text = "Height : " + height.ToString();
                }
            }

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers);



            //if(Input.GetKeyDown(KeyCode.DownArrow))
            //{

            //}
            Vector2 moveDir = new Vector2(Input.GetAxisRaw("Horizontal") * Speed, rb.velocity.y);
            rb.velocity = moveDir;

            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (isFlipped == true)
                    flip();
                //rb.MovePosition(rb.position + velocityX * Time.fixedDeltaTime);
                animator.SetTrigger("RunRight");
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (isFlipped == false)
                    flip();
                //rb.MovePosition(rb.position - velocityX * Time.fixedDeltaTime);
                animator.SetTrigger("RunRight");
            }

            if (height >= 68)
            {
                winText.text = "Clear!!";
            }
            else if (height < 0)
            {
                winText.text = "You've got failed!!";
            }
        }
        else
        {
            SceneManager.LoadScene("Gameover");
        }
       
    }

    void flip()
    {
        Vector3 playerScale = transform.localScale;
        playerScale.x = playerScale.x * -1;
        transform.localScale = playerScale;
        isFlipped = !isFlipped;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
