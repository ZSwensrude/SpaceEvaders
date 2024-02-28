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
    GameSettings gameSettings;

    float score = 0;
    int highScore = 0;
    bool incrementScore = false;
    float pointsPerSecond = 5;

    int scoreMultiplier = 0;
    public int ScoreMultiplier { get => scoreMultiplier; set => scoreMultiplier = value; }

    private void Awake()
    {
        scoreText = ScoreObject.GetComponent<TextMeshProUGUI>();
        scoreMultiplierText = ScoreMultiplierObject.GetComponent<TextMeshProUGUI>();
     
        ScoreMultiplier = gameSettings.ScoreMultiplier;
        incrementScore = gameSettings.IncrementScore;
    }

    // Update is called once per frame
    void Update()
    {

        if(incrementScore)
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
    }


    void CheckHighScore()
    {
        
    }
    
}
