using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Accord.Math;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer skybox;

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
    GameObject Tutorial;

    [SerializeField]
    GameSettings gameSettings;

    [SerializeField]
    GameObject Player;

    [SerializeField]
    private float frameWeight;

    [SerializeField]
    private bool tutorial;

    float score = 0;
    int highScore = 0;
    float pointsPerSecond = 0;

    float distance = 0;
    float nextStopDistance = 0;
    float maxStopDistance = 600;
    // default distance to next stop
    readonly int stopDistance = 100;
    int stopsHit = 1;
    int timeToWait = 5;

    float speedMultiplier = 1.2f;

    bool incrementScore = false;
    bool bossActive = false;

    [SerializeField]
    float scoreMultiplier = 1;
    public float ScoreMultiplier { get => scoreMultiplier; set => scoreMultiplier = value; }

    [SerializeField]
    float distanceMultiplier = 1;
    public float DistanceMultiplier { get => distanceMultiplier; set => distanceMultiplier = value; }

    [SerializeField]
    private GameObject bossShip;
    [SerializeField]
    private AudioSource mainLoop;
    [SerializeField]
    private AudioSource bossIntro;
    [SerializeField]
    private AudioSource bossLoop;

    [SerializeField]
    private BossHandler bossHandler;

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
        skybox.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Animated_Gas_Planet.mp4");
        skybox.Play();
        if(!tutorial)
            StartCoroutine("TutorialText");

        scoreText = ScoreObject.GetComponent<TextMeshProUGUI>();
        scoreMultiplierText = ScoreMultiplierObject.GetComponent<TextMeshProUGUI>();

        highscoreText = HighScoreObject.GetComponent<TextMeshProUGUI>();
        if(!tutorial)
            highScore = PlayerPrefs.GetInt("HighScore", 0);
        highscoreText.text = highScore.ToString();

        distanceText = DistanceObject.GetComponent<TextMeshProUGUI>();
        nextStopText = NextStopObject.GetComponent<TextMeshProUGUI>();

        // set initial stop distance
        nextStopDistance = stopDistance;

        bossShip.SetActive(false);

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
        distance += pointsPerSecond * DistanceMultiplier * Time.deltaTime / 10;
        nextStopDistance -= pointsPerSecond * DistanceMultiplier * Time.deltaTime / 10;

        if (nextStopDistance <= 0)
        {
            gameSettings.IncrementScore = false;
            spawner.StopSpawning();
            stopsHit++;
            // max out stop distance at 600 (anything else feels too long)
            if (stopsHit < 6)
                nextStopDistance = stopDistance * stopsHit;
            else
                nextStopDistance = maxStopDistance;
            // boss battle every 3 stops 
            if (stopsHit % 3 == 0 && !bossActive && !tutorial)
            {
                StartCoroutine("StartBossBattle");
            } else
            {
                // give player break before starting again
                StartCoroutine("WaitBetweenStops");
            }

        }

        distanceText.text = ((int)distance).ToString() + "ly";
        nextStopText.text = ((int)nextStopDistance).ToString() + "ly";

    }
    
    private IEnumerator TutorialText()
    {
        if (!tutorial)
        {
            StartCoroutine("TutorialFadeIn");
            yield return new WaitForSeconds(5);
            StartCoroutine("TutorialFadeOut");
        }

        yield return null;
    }

    private IEnumerator TutorialFadeIn()
    {
        while(Tutorial.GetComponent<CanvasGroup>().alpha != 1)
        {            
            Tutorial.GetComponent<CanvasGroup>().alpha += 0.5f*Time.deltaTime;
            yield return new WaitForSeconds(0.001f);

        }

        yield return null;
    }

    private IEnumerator TutorialFadeOut()
    {

        while(Tutorial.GetComponent<CanvasGroup>().alpha != 0)
        {
            Tutorial.GetComponent<CanvasGroup>().alpha -= 0.5f*Time.deltaTime;
            yield return new WaitForSeconds(0.001f);
        }

        yield return null;
    }

    private IEnumerator WaitBetweenStops ()
    {
        if (bossActive)
        {
            spawner.StopBossBattle();
            bossActive = false;
            spawner.StopBossBattle();
            bossLoop.Stop();
            mainLoop.Play();
            bossHandler.FlyAway();
        }
        gameSettings.AsteroidSpeed *= speedMultiplier;
        gameSettings.AsteroidSpawnInterval /= speedMultiplier;
        ScoreMultiplier *= speedMultiplier;
        DistanceMultiplier *= speedMultiplier;
        //50/50 chance to add another asteroid to max asteriods to spawn
        if (UnityEngine.Random.value < 0.5)
            gameSettings.AsteroidsInGroup += 1; 
        
        StartCoroutine("UpdateWeights");
        yield return new WaitForSeconds(timeToWait);
        
        gameSettings.IncrementScore = true;
        spawner.RunSpawner = true;
        spawner.UpdateRate = true;
    }

    private IEnumerator StartBossBattle()
    {
        mainLoop.Stop();
        bossIntro.Play();
        Invoke("StartLoop", 15f);
        bossShip.SetActive(true);
        bossHandler.FlyIn();

        float bossIntroTime = 3.5f;
        yield return new WaitForSeconds(timeToWait + bossIntroTime);

        Debug.Log("Boss time!!");
        bossActive = true;
        spawner.StartBossBattle();
        gameSettings.IncrementScore = true;

    }

    private void StartLoop()
    {
        bossIntro.Stop();
        bossLoop.Play();
    }

    private void IncrementScore()
    {
        // increment score based on points/sec * multiplier
        score += pointsPerSecond * ScoreMultiplier * Time.deltaTime;
        scoreText.text = ((int)score).ToString();
        // update multiplier in case it was changed
        scoreMultiplierText.text = "x" + scoreMultiplier.ToString("F1");

        // if new highscore
        if (score > highScore && !tutorial)
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
            if (i < 3)
            {
                muY += -1*asteroidWeighting[i];
            }
            if (i >= 6)
            {
                muY += asteroidWeighting[i];
            }
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
