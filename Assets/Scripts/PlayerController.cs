using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputAction moveForward;
    [SerializeField] InputAction lookPosition;
    public float thrustForce = 7f;
    public float maxSpeed = 5f;
    Rigidbody2D rb;
    public GameObject flame;
    private float elapsedTime = 0f;
    private float score = 0f;
    private float scoreMultiplier = 10f;
    public UIDocument uIDocument;
    private Label scoreText;
    private Label highscoreText;
    [SerializeField] int highscore;
    private const string HighScoreKey = "HighScore";
    [SerializeField] GameObject explosionEffect;
    [SerializeField] Button restartButton; // label "RestartButton"
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        moveForward.Enable();
        lookPosition.Enable();
        scoreText = uIDocument.rootVisualElement.Q<Label>("ScoreLabel");
        highscoreText = uIDocument.rootVisualElement.Q<Label>("HighScoreLabel");
        highscore = PlayerPrefs.GetInt(HighScoreKey);
        highscoreText.text = "Highscore: " + highscore;
        restartButton = uIDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None; // hide button 
        restartButton.clicked += reloadScene; // add handler, magic overloading
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        scoreText.text = "Score: " + score;
        if (moveForward.IsPressed())
        {
            // Calculate mouse direction
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(lookPosition.ReadValue<Vector2>()); //Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePosition - transform.position).normalized;


            // move player to direction of mouse 
            transform.up = direction;
            rb.AddForce(direction * thrustForce);

            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }

        if (moveForward.WasPressedThisFrame())
        {
            flame.SetActive(true);
        }
        else if (moveForward.WasReleasedThisFrame())
        {
            flame.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
        restartButton.style.display = DisplayStyle.Flex;
        gameOver();
    }

    void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void gameOver()
    {
        if (score > highscore)
        {
            int intScore = Mathf.FloorToInt(score);
            PlayerPrefs.SetInt(HighScoreKey, intScore);
            highscoreText.text = "New Highscore!  " + intScore;
        }
    }
}
