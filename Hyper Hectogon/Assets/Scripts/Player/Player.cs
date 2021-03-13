using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Serialize Fields
    [SerializeField] private SelectionSpawner selectionSpawner;
    [SerializeField] private HorizontalDisabler horizontalDisabler;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private Sprite[] playerSprites;
    [SerializeField] private int[] stageChangeCount;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private SoundManager soundManager;

    // Temporary Property
    [SerializeField] private Color[] color;

    // Private Variables
    private PlayerStat playerStat;
    private SpriteRenderer spriteRenderer;
    private int stageCount = 0;
    private float speed = 5f;
    private float healthDecreaseRate = -4.0f;
    private float comboDecreaseRate = -10.0f;
    private int stack = 0;
    private readonly int maxStack = 3;
    private float maxSpeed = 15f;
    //private int enemyStack = 0; // For Sound Effect
    //private readonly int maxEnemyStack = 3;

    private void Start()
    {
        playerStat = GetComponent<PlayerStat>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (playerStat.Health == 0)
        {
            GameOverPanel.SetActive(true);
            return;
        }
        transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);

        #region Movement for PC

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(-1); //Move Left
            selectionSpawner.SpawnLeftOrRight(transform.position, -1); // Spawn Left
            selectionSpawner.SpawnUp(transform.position); // Spawn Up
        }
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(1); //Move Right
            selectionSpawner.SpawnLeftOrRight(transform.position, 1); // Spawn Right
            selectionSpawner.SpawnUp(transform.position); // Spawn Up
        }

        #endregion

        #region Movement for Mobile

        if (Input.touchCount > 0)
        {
            selectionSpawner.SpawnUp(transform.position); // Spawn Up

            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPos = Camera.main.ScreenToViewportPoint(touch.position);
                if (touchPos.x >= 0.5)
                {
                    Move(1); // Move Right
                    selectionSpawner.SpawnLeftOrRight(transform.position, 1); // Spawn Right
                }
                else if (touchPos.x < 0.5)
                {
                    Move(-1); // Move Left
                    selectionSpawner.SpawnLeftOrRight(transform.position, -1); // Spawn Left
                }
            }
        }

        #endregion

        // Decrease Health
        playerStat.Health += healthDecreaseRate * Time.deltaTime;
        // Decrease Combo Gauge
        scoreManager.UpdateCombo(comboDecreaseRate * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        selectionSpawner.SpawnUp(transform.position); // Spawn Up
        string clip = "";

        if (collision.tag == "Upgrade")
        {
            scoreManager.UpdateCombo(-50);
            // decide which clip to play by stack value
            switch (stack)
            {
                case 0:
                    clip = "upgrade0";
                    break;
                case 1:
                    clip = "upgrade1";
                    break;
                case 2:
                    clip = "upgrade2";
                    break;
                case 3:
                    clip = "upgrade3";
                    break;
            }
            //clip = "upgrade0";
            IncreaseStack();
        }
        else if (collision.tag == "HealthUp")
        {
            scoreManager.UpdateCombo(-50);
            playerStat.Health += 50;

            #region sound test

            //float test = Random.value;
            //if (test < 0.5f)
            //{
            //    clip = "healthUp0";
            //}
            //else
            //{
            //    clip = "healthUp1";
            //}

            #endregion
            clip = "healthUp0";
        }
        else if (collision.tag == "Enemy")
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            playerStat.Health -= enemy.Combat(playerStat.Tier);
            scoreManager.UpdateScore(enemy.Score);
            scoreManager.UpdateCombo(100); // NEED MODIFICATION

            #region sound test

            //enemyStack = (enemyStack + 1) % maxEnemyStack;

            //switch(enemyStack)
            //{
            //    case 0:
            //        clip = "laser0";
            //        break;
            //    case 1:
            //        clip = "laser1";
            //        break;
            //    case 2:
            //        clip = "laser2";
            //        break;
            //}
            clip = "laser0";

            #endregion
        }
        else if(collision.tag == "Obstacle")
        {
            playerStat.Health -= 20;
            scoreManager.UpdateCombo(-100);

            #region sound test

            //float test = Random.value;
            //if (test < 0.5f)
            //{
            //    clip = "obstacle0";
            //}
            //else
            //{
            //    clip = "obstacle1";
            //}

            #endregion
            clip = "obstacle0";
        }

        collision.gameObject.SetActive(false);

        soundManager.PlaySound(clip); // play sound effect
        DisableEntireLine();
        UpdateStage();
    }

    /// <summary>
    /// Transforms Player to left or right position
    /// </summary>
    /// <param name="direction">1 for right, -1 for left</param>
    private void Move(int direction)
    {
        float distance = 2.25f;
        transform.position = new Vector2(transform.position.x + distance * direction, transform.position.y);
    }


    private void UpdateStage()
    {
        stageCount++;
        if (stageCount >= stageChangeCount[playerStat.Stage])
        {
            ChangeSpeed(1f);
            playerStat.Stage++;
            stageCount = 0;
        }
    }

    private void IncreaseStack()
    {
        // Stack Sprite Increase Effect

        stack++; // Increase stack

        // Change Tier
        if(stack > maxStack)
        {
            UpgradeTier();
            stack = 0;
        }
    }

    private void UpgradeTier()
    {
        playerStat.Tier++;
        //spriteRenderer.sprite = playerSprites[playerStat.Tier];
        spriteRenderer.material.color = color[playerStat.Tier];
    }

    private void ChangeSpeed(float deltaSpeed)
    {
        speed += deltaSpeed;
        if (speed >= maxSpeed)
        {
            speed = maxSpeed;
        }
    }

    private void DisableEntireLine()
    {
        horizontalDisabler.transform.position = new Vector2(transform.position.x, transform.position.y + 1.0f);
    }
}
