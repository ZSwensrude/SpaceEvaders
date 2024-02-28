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

    int score = 0;
    int highScore = 0;
    int scoreMultiplier = 0;

    private void Awake()
    {
        scoreText = ScoreObject.GetComponent<TextMeshProUGUI>();
        scoreMultiplierText = ScoreMultiplierObject.GetComponent<TextMeshProUGUI>();
     
        scoreMultiplier = gameSettings.ScoreMultiplier;

    }

    // Update is called once per frame
    void Update()
    {
        scoreMultiplier++;
        Debug.Log("scoreMultiplier" + scoreMultiplier);
        scoreMultiplierText.text = "x" + scoreMultiplier.ToString();
        
    }

    /*
    private void IncrementScore()
    {
        score += score * scoreMultiplier;
    }


    void CheckHighScore()
    {
        if (score > )
    }
    */
}
