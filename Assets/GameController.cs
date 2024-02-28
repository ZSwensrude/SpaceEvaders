using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

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
    bool incrementScore = false;
    float pointsPerSecond = 0;

    int scoreMultiplier = 0;
    public int ScoreMultiplier { get => scoreMultiplier; set => scoreMultiplier = value; }

    private void Awake()
    {
        scoreText = ScoreObject.GetComponent<TextMeshProUGUI>();
        scoreMultiplierText = ScoreMultiplierObject.GetComponent<TextMeshProUGUI>();
        ScoreMultiplier = gameSettings.ScoreMultiplier;

        highscoreText = HighScoreObject.GetComponent<TextMeshProUGUI>();
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highscoreText.text = highScore.ToString();
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
        }
        
    }

    
    private void IncrementScore()
    {
        // increment score based on points/sec * multiplier
        score += pointsPerSecond * ScoreMultiplier * Time.deltaTime;
        scoreText.text = ((int)score).ToString();
        // update multiplier in case it was changed
        scoreMultiplierText.text = "x" + scoreMultiplier.ToString();

        // if new highscore
        if (score > highScore)
        {
            highScore = (int)score;
            highscoreText.text = highScore.ToString();
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }


    
}
