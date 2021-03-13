using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    private HangOn hangOn;
    private bool __isLadder = false;
    private Rigidbody2D myBody;
    private float ladderSpeed = 3f;
    private Player player;
    public bool onLadder { get { return __isLadder; } set { __isLadder = value; } }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        player = gameObject.GetComponent<Player>();
        hangOn = gameObject.GetComponent<HangOn>();
        myBody = gameObject.GetComponent<Rigidbody2D>();
    }
    /// <summery>
    /// Enter to ladder
    /// </summery>
    /// <param type="GameObject">because ladder position</param>
    public void EnterLadder(in GameObject other) {
        if (hangOn.isHangOn) return;
        onLadder = true;
        myBody.gravityScale = 0f;
        myBody.velocity = Vector3.zero;
        transform.position = new Vector2(other.transform.position.x, transform.position.y);
    }

    /// <summery>
    /// movement on ladder
    /// </summery>
    public void LadderMove()
    {
        Vector2 move = player.leftJoystickVector;
        move.x = 0;
        transform.Translate(move * ladderSpeed * Time.deltaTime);
    }

    /// <summery>
    /// exit from ladder
    /// </summery>
    public void ExitLadder() {
        myBody.gravityScale = 3f;
        onLadder = false;
    }
}
