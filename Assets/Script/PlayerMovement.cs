using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Scripting.APIUpdating;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerMovement : MonoBehaviour
{
    private bool inputEnabled = true;
    public float moveSpeed;
    public Rigidbody2D rb;
    private Vector2 moveDir;

    public float timerDuration = 20f;
    private float timer;

    public TextMeshProUGUI timerText;
    private bool isSwallowed = false;
    private bool hasWon = false;

    public GameObject GOPanal;
    public GameObject GOPanall;
    public GameObject obstacleText;

    public TextMeshProUGUI scoreText;

    public GameObject GOSpeedPanel;
    public float speedIncreaseAmount = 10f; // Amount by which the ball's speed will increase


    int score = 0;

    // Speed at which the player moves.
    public float speed = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GOPanal.SetActive(false);
        GOPanall.SetActive(false);
        obstacleText.SetActive(false);
        GOSpeedPanel.SetActive(false);
        Time.timeScale = 1f;


        timer = timerDuration*10;
        UpdateTimerText();
        DisplayScore();

        InvokeRepeating("CountdownTimer", 1f, 1f);

        
    }



    public void AddScore(int value)
    {
        score += value;
        DisplayScore();
    }
    public void DisplayScore() {
        scoreText.text = "Coins: " + score;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Obstacles"))
        {
            if (score == 0) {
                Lose();
            }
            else
            {
                score--;
                DisplayScore();
                StartCoroutine(DisplayObstacleMessage());
            }   
        }
    }

    private IEnumerator DisplayObstacleMessage()
    {
        obstacleText.SetActive(true);

        yield return new WaitForSeconds(3f);

        obstacleText.SetActive(false);
    }


    private void Update()
    {

        if (inputEnabled)
        {
            // Process input logic here
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            Vector2 moveDir = new Vector2(moveX, moveY).normalized;
            // Apply movement
            transform.Translate(moveDir * moveSpeed * Time.deltaTime);
        }

    }
    void FixedUpdate()
    {
        if (!isSwallowed)
            Move();   
    }
    void ProcessInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDir = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDir.x * moveSpeed , moveDir.y * moveSpeed);
    }


    private IEnumerator SwallowBall()
    {
        isSwallowed = true;

        // Gradually scale down the ball's size
        float scale = transform.localScale.x;
        while (scale > 0)
        {
            scale -= Time.deltaTime;
            transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

       Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Hole"))
        {
            if (!isSwallowed)
            {
                StopTimer();
                hasWon = true;
                Win();
                StartCoroutine(SwallowBall());
            }
        }
        if (other.gameObject.CompareTag("Coins"))
        {
            AddScore(1);  // Add 1 to the score
            other.gameObject.SetActive(false); // Destroy the coin
        }

        if (other.gameObject.CompareTag("PowerUp"))
        {
            // Increase the ball's speed
            IncreaseSpeed(speedIncreaseAmount);
            StartCoroutine(DisplaySpeedBoostMessage());

            // Disable or destroy the speed boost object
            other.gameObject.SetActive(false); // or Destroy(collision.gameObject);
        }

    }

    public void IncreaseSpeed(float amount)
    {
        speed += amount;
    }


    private IEnumerator DisplaySpeedBoostMessage()
    {
        GOSpeedPanel.SetActive(true);

        yield return new WaitForSeconds(3f);

        GOSpeedPanel.SetActive(false);
    }


    private void CountdownTimer()
    {
        timer--;
        if (timer <= 0)
        {
            StopTimer();
            if (!isSwallowed)
                Lose();
        }

        UpdateTimerText();
    }

    private void StopTimer()
    {
        CancelInvoke("CountdownTimer");
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public GameOverScreen GameOverScreen;
    private void Win()
    {
        SwallowBall();
        GOPanall.SetActive(true);
        Time.timeScale = 0f;
        GameOverScreen.NextButton();
    }
    private void Lose()
    {
        inputEnabled = false;
        GOPanal.SetActive(true);
        Time.timeScale = 0f;
        GameOverScreen.RestartButton();
    }



}