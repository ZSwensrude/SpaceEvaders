using System;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.Distributions;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidSpawner : MonoBehaviour
{
    private bool printLogs;

    [SerializeField]
    private GameSettings gameSettings;
    [SerializeField]
    private List<GameObject> asteroidPrefabs;
    [SerializeField]
    private List<GameObject> breakableAsteroidPrefabs;

    [SerializeField]
    private List<GameObject> bossBombsPrefabs;

    // used in script, also used to set default values
    [SerializeField]
    private float spawnInterval;
    [SerializeField]
    private int numToSpawn;
    [SerializeField]
    private float asteroidSpeed;

    private float gridLength;

    private bool runSpawner = true;

    private float breakableChance = 0.10f;

    private bool bossBattle = false;

    public bool RunSpawner { get => runSpawner; set => runSpawner = value; }


    //This is the default rate used for generating asteroids via the Poisson Process
    //This value represents the amount of asteroids per second in a given square on average
    [SerializeField]
    private double defaultRate = 0.05;
    //updateRate used to check if we should change the rate for the asteroid
    //spawning
    private bool updateRate = false;
    //UpdateRate creates getters and setters for updateRate that can be
    //accessed outside of this class
    public bool UpdateRate { get => updateRate; set => updateRate = value; }
    private void Awake()
    {
        printLogs = gameSettings.PrintLogs;

        // reset all the scriptable object values to default
        gameSettings.AsteroidSpeed = asteroidSpeed;
        gameSettings.AsteroidSpawnInterval = spawnInterval;
        gameSettings.AsteroidsInGroup = numToSpawn;

        // get gridLength from game settings
        gridLength = gameSettings.GridLength;
    }

    void Start()
    {
        Debug.Log("AsteroidSpawner Start");
        // turn off spawner at start to do tutorial
        RunSpawner = false;
        StartCoroutine(SpawnLoop());
    }


    private int[] GetAsteroids(int numAsteroids)
    {
        // final positions, 0 denotes no asteroid, 1 denotes asteroid
        int[] positions = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        List<int> possible = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        int breakables = 0;
        int maxBreakables = 3;

        int randIndex;

        for (int i = 0; i < numAsteroids; i++)
        {
            randIndex = Random.Range(0, possible.Count);
            // spawn breakable asteroids 10% of the time, or when there is 9 asteroids, with a max number of two
            if (breakables < maxBreakables && (Random.value < breakableChance || numAsteroids == 9))
            {
                positions[possible[randIndex]] = 2; // 2 for breakable asteroid
                breakables++;
            }
            else
            {
                positions[possible[randIndex]] = 1; // 1 for normal asteroid
            }
            possible.RemoveAt(randIndex);
        }

        return positions;
    }

    IEnumerator TutorialSpawn()
    {
        // send one asteroid out in the middle of the screen
        int[] locations = new int[9] { 0, 0, 0, 0, 1, 0, 0, 0, 0 };
        SpawnAsteroids(locations);
        // give them some time
        yield return new WaitForSeconds(3);

        // spawn a full layer of breakable asteroids
        locations = new int[9] { 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        SpawnAsteroids(locations);

        yield return new WaitForSeconds(3);
        RunSpawner = true;
        UpdateRate = true;
    }


    IEnumerator SpawnLoop()
    {
        StartCoroutine(TutorialSpawn());

        while (true)
        {
            // get values from scriptable object
            asteroidSpeed = gameSettings.AsteroidSpeed;
            spawnInterval = gameSettings.AsteroidSpawnInterval;
            // spawn random amount of asteroids between 5 and AsteroidsInGroup + 1 (exclusive)
            numToSpawn = Random.Range(5, gameSettings.AsteroidsInGroup + 1);


            if (RunSpawner)
            {
                //int[] locations = GetAsteroids(numToSpawn);
                int[] locations = GetAsteroids(numToSpawn);

                if (printLogs)
                {
                    String log = "Random asteroids list: [";
                    foreach (var item in locations)
                    {
                        log = log + item.ToString() + " ";
                    }
                    log = log + "]";
                    Debug.Log(log);
                }

                // spawn asteroids
                SpawnAsteroids(locations);
            }

            // increase speed of next asteroids (proof that we can change the speed while playing)
            // gameSettings.AsteroidSpeed = asteroidSpeed + 10f;

            // wait for the interval before running again
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void StartBossBattle()
    {
        // stop normal spawning
        RunSpawner = false;
        StopCoroutine("StatisticalAsteroidGen");
        // start boss battle
        bossBattle = true;

        InitDistros(defaultRate);
    }

    public void StopBossBattle()
    {
        // start boss battle
        bossBattle = false;
    }


    private void SpawnAsteroids(int[] locations)
    {
        int x = -1;
        int y = -1;
        Vector3 spawnPosition;

        for (int i = 1; i < locations.Length + 1; i++)
        {
            spawnPosition = new Vector3((transform.position.x - (x * gridLength)), (transform.position.y - (y * gridLength)), (transform.position.z));

            if (printLogs)
                Debug.Log("spawnPosition: " + spawnPosition.ToString());

            // if the num is a 1 we want to spawn a normal asteroid
            if (locations[i - 1] == 1)
            {
                // randomly get position to spawn asteroid 
                Instantiate(asteroidPrefabs[UnityEngine.Random.Range(0, asteroidPrefabs.Count)], spawnPosition, Random.rotation).GetComponent<Asteroid>();
            }
            // else if the num is a 2 we want to spawn a breakable one
            else if (locations[i - 1] == 2)
            {
                Instantiate(breakableAsteroidPrefabs[UnityEngine.Random.Range(0, breakableAsteroidPrefabs.Count)], spawnPosition, Random.rotation).GetComponent<Asteroid>();
            }

            x++;

            if (i % 3 == 0)
            {
                // reset x location and decrease y location
                x = -1;
                y++;
            }
        }
    }

    /// <summary>
    /// Initialize Distributions for each square in the grid. Uses Poisson
    /// Process to generate arrival times for each asteroid using Exponential
    /// distribution.
    /// Takes rate as a parameter to determine the number of asteroids in a
    /// given square each second (expected value anyway). This rate is
    /// also the mean lambda of the Poisson distribution this models.
    /// </summary>
    /// <param name="rate">Double representing the rate at which asteroids come
    /// per second.</param>
    public void InitDistros(double rate)
    {
        Vector3 spawnPosition;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                spawnPosition = new Vector3(transform.position.x - (j * gridLength), transform.position.y - (i * gridLength), transform.position.z);
                StartCoroutine(StatisticalAsteroidGen(spawnPosition, rate));
            }

        }
    }

    /// <summary>
    /// Coroutine that generates asteroids according to an exponential distribution.
    /// Randomly samples values that represent the number of seconds before the
    /// next asteroid spawns in that square.
    /// </summary>
    /// <param name="spawnPosition">Position of the asteroid to be spawned</param>
    /// <param name="rate">Rate at which asteroids should be generated per second</param>
    /// <returns>yield return WaitForSeconds</returns>
    IEnumerator StatisticalAsteroidGen(Vector3 spawnPosition, double rate)
    {

        var currentExpo = new Exponential(rate);

        while (RunSpawner || bossBattle){

            //Verify that after waiting, we're still allowed to generate the asteroid
            if(RunSpawner)
            {
                yield return new WaitForSeconds((float) currentExpo.Sample());
                Instantiate(breakableAsteroidPrefabs[UnityEngine.Random.Range(0, breakableAsteroidPrefabs.Count)], spawnPosition, Random.rotation).GetComponent<Asteroid>();
            } else if (bossBattle)
            {
                int reduceInterval = 5;
                yield return new WaitForSeconds((float) currentExpo.Sample() / reduceInterval);
                Instantiate(bossBombsPrefabs[UnityEngine.Random.Range(0, bossBombsPrefabs.Count)], spawnPosition, Random.rotation).GetComponent<Asteroid>();
            }
        }

        StopCoroutine("StatisticalAsteroidGen");

    }

    /// <summary>
    /// Currently Update only updates the rate value for the exponential
    /// distributions.
    /// </summary>
    public void Update()
    {
        if(UpdateRate && !bossBattle){
            defaultRate += 0.025;
            InitDistros(defaultRate);
            UpdateRate = false;
        }
    }

}