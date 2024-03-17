using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    AsteroidSpawner spawner;

    [SerializeField]
    GameObject ScoreObject;
    TextMeshProUGUI scoreText;

    [SerializeField]
    GameObject ScoreMultiplierObject;
    TextMeshProUGUI scoreMultiplierText;

    [SerializeField]
    GameObject HighScoreObject;
    TextMeshProUGUI highscoreText;

    [SerializeField]
    GameObject DistanceObject;
    TextMeshProUGUI distanceText;

    [SerializeField]
    GameObject NextStopObject;
    TextMeshProUGUI nextStopText;

    [SerializeField]
    GameSettings gameSettings;

    float score = 0;
    int highScore = 0;
    float pointsPerSecond = 0;

    float distance = 0;
    float nextStopDistance = 0;
    // default distance to next stop
    readonly int stopDistance = 100;
    int stopsHit = 1;
    int timeToWait = 5;

    float speedMultiplier = 1.2f;

    bool incrementScore = false;

    [SerializeField]
    float scoreMultiplier = 1;
    public float ScoreMultiplier { get => scoreMultiplier; set => scoreMultiplier = value; }

    private void Awake()
    {
        scoreText = ScoreObject.GetComponent<TextMeshProUGUI>();
        scoreMultiplierText = ScoreMultiplierObject.GetComponent<TextMeshProUGUI>();

        highscoreText = HighScoreObject.GetComponent<TextMeshProUGUI>();
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highscoreText.text = highScore.ToString();

        distanceText = DistanceObject.GetComponent<TextMeshProUGUI>();
        nextStopText = NextStopObject.GetComponent<TextMeshProUGUI>();

        // set initial stop distance
        nextStopDistance = stopDistance;

        // make sure everything is running (might be false if stopped during a break)
        gameSettings.IncrementScore = true;
        spawner.RunSpawner = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // update from game settings (in case they changed elsewhere)
        incrementScore = gameSettings.IncrementScore;
        pointsPerSecond = gameSettings.AsteroidSpeed;

        if (incrementScore)
        {
            IncrementScore();
            IncrementDistance();
        }

        
    }

    private void IncrementDistance()
    {
        distance += pointsPerSecond * ScoreMultiplier * Time.deltaTime / 10;
        nextStopDistance -= pointsPerSecond * ScoreMultiplier * Time.deltaTime / 10;

        if (nextStopDistance <= 0)
        {
            gameSettings.IncrementScore = false;
            spawner.RunSpawner = false;
            stopsHit++;
            nextStopDistance = stopDistance * stopsHit;

            // give player break before starting again
            StartCoroutine("WaitBetweenStops");
            
        }

        distanceText.text = ((int)distance).ToString() + "ly";
        nextStopText.text = ((int)nextStopDistance).ToString() + "ly";

    }

    private IEnumerator WaitBetweenStops ()
    {
        gameSettings.AsteroidSpeed *= speedMultiplier;
        gameSettings.AsteroidSpawnInterval /= speedMultiplier;
        ScoreMultiplier *= speedMultiplier;
        //50/50 chance to add another asteroid to max asteriods to spawn
        if(Random.value < 0.5)
            gameSettings.AsteroidsInGroup += 1; 

        yield return new WaitForSeconds(timeToWait);
        gameSettings.IncrementScore = true;
        spawner.RunSpawner = true;
        spawner.UpdateRate = true;
    }

    private void IncrementScore()
    {
        // increment score based on points/sec * multiplier
        score += pointsPerSecond * ScoreMultiplier * Time.deltaTime;
        scoreText.text = ((int)score).ToString();
        // update multiplier in case it was changed
        scoreMultiplierText.text = "x" + scoreMultiplier.ToString("F1");

        // if new highscore
        if (score > highScore)
        {
            highScore = (int)score;
            highscoreText.text = highScore.ToString();
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }


    
}
