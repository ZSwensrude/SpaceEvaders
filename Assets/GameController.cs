using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Accord.Math;

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

    [SerializeField]
    GameObject Player;

    [SerializeField]
    private float frameWeight;

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

    //at beginning of game, each position is equally as likely to generate an asteroid
    private float[] asteroidWeighting = {0, 0, 0, 0, 0, 0, 0, 0, 0};

    [SerializeField]
    private double sigma = 1;

    // private float[] gaussArr = {(float)Normal.Gaussian2D(sigma, -1, -1),
    //                             (float)Normal.Gaussian2D(sigma, 0, -1),
    //                             (float)Normal.Gaussian2D(sigma, 1, -1),
    //                             (float)Normal.Gaussian2D(sigma, -1, 0),
    //                             (float)Normal.Gaussian2D(sigma, 0, 0),
    //                             (float)Normal.Gaussian2D(sigma, 0, 1),
    //                             (float)Normal.Gaussian2D(sigma, -1, 1),
    //                             (float)Normal.Gaussian2D(sigma, 0, 1),
    //                             (float)Normal.Gaussian2D(sigma, 1, 1)};

    private (double,double)[] gaussArr = {(-1,-1),
                                        (0, -1),
                                        (1, -1),
                                        (-1, 0),
                                        (0, 0),
                                        (1, 0),
                                        (-1, 1),
                                        (0, 1),
                                        (1, 1)};

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

        StartCoroutine("GetPlayerWeights");
        
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
        if(UnityEngine.Random.value < 0.5)
            gameSettings.AsteroidsInGroup += 1; 
        
        StartCoroutine("UpdateWeights");
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

    /// <summary>
    /// Grab player position in grid coordinates and use that to update the asteroid
    /// weighting later. Weights should sum to 1
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetPlayerWeights()
    {
        (int x, int y) = Player.GetComponent<Movement>().GetPlayerGridPos();

        // convert from grid coordinates to index coordinates
        x++; y++; y*=3;

        // add weight to current pos
        asteroidWeighting[x + y] += frameWeight;

        yield return null;
    }

    public IEnumerator UpdateWeights()
    {
        double muX = 0;
        double muY = 0;
        float[] result = new float[9];
        double sum = asteroidWeighting.Sum();

        for (int i=0; i < asteroidWeighting.Length; i++) 
        {
            muX += ((i%3)-1)*asteroidWeighting[i];
            muY += (i-1)%3*asteroidWeighting[i];
        }

        for (int i=0; i < result.Length; i++)
        {
            result[i] = (float)Normal.Gaussian2D(sigma, gaussArr[i].Item1 + muX/sum, gaussArr[i].Item2 + muY/sum);
            Debug.Log(result[i]);
        }

        
        spawner.UpdateWeights = result;

        yield return null;
    }

    /// <summary>
    /// Get probability weights for asteroids (all of them)
    /// </summary>
    /// <returns>probability weights for each zone as array of floats</returns>
    public float[] GetWeights()
    {
        
        return asteroidWeighting;

    }

    /// <summary>
    /// Get specific weight for asteroid weighting
    /// </summary>
    /// <param name="index">int for index of the array</param>
    /// <returns>the value of the index specified</returns>
    /// <exception cref="ArgumentOutOfRangeException">index not valid</exception>
    public float GetWeight(int index)
    {
        try
        {
            return asteroidWeighting[index];
        }
        catch (IndexOutOfRangeException e)
        {
            throw new ArgumentOutOfRangeException(
                "Index (" + index + ") given is not valid.", e
            );
        }

    }
    /// <summary>
    /// Set all weights of asteroidWeighting
    /// </summary>
    /// <param name="weights">array of weights to set</param>
    public void SetWeights(float[] weights)
    {
        for(int i=0; i < weights.Length; i++)
        {
            asteroidWeighting[i] = weights[i];
        }
    }
    
    /// <summary>
    /// Set specific index of asteroidWeighting to supplied weight
    /// </summary>
    /// <param name="weight">float to be set as weight for specified index</param>
    /// <param name="index">index of zone to have weight set</param>
    /// <exception cref="ArgumentOutOfRangeException">invalid index supplied</exception>
    public void SetWeight(float weight, int index)
    {

        try
        {
            asteroidWeighting[index] = weight;
        }
        catch (IndexOutOfRangeException e)
        {
            throw new ArgumentOutOfRangeException(
                "Index (" + index + ") given is not valid.", e
            );
        }

    }
    
}
