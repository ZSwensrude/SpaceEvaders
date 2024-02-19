using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidSpawner : MonoBehaviour
{
    private bool printLogs;

    [SerializeField]
    private GameSettings gameSettings;
    [SerializeField]
    private List<GameObject> asteroidPrefabs;

    // used in script, also used to set default values
    [SerializeField]
    private float spawnInterval;
    [SerializeField]
    private int numToSpawn;
    [SerializeField]
    private float asteroidSpeed;

    private float gridLength;

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
        StartCoroutine(SpawnLoop());
    }


    private int[] GetAsteroids(int numAsteroids)
    {
        // final positions, 0 denotes no asteroid, 1 denotes asteroid
        int[] positions = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        List<int> possible = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

        int randIndex;

        for (int i = 0; i < numAsteroids; i++)
        {
            randIndex = Random.Range(0, possible.Count);
            positions[possible[randIndex]] = 1;
            possible.RemoveAt(randIndex);
        }

        return positions;
    }


    IEnumerator SpawnLoop ()
    {
        while (true)
        {
            // get values from scriptable object
            asteroidSpeed = gameSettings.AsteroidSpeed;
            spawnInterval = gameSettings.AsteroidSpawnInterval;
            numToSpawn = gameSettings.AsteroidsInGroup;
    
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

            // increase speed of next asteroids (proof that we can change the speed while playing)
            gameSettings.AsteroidSpeed = asteroidSpeed + 10f;

            // wait for the interval before running again
            yield return new WaitForSeconds(spawnInterval);
        }
    }


    private void SpawnAsteroids (int[] locations)
    {
        int x = -1;
        int y = -1;
        Vector3 spawnPosition;

        for (int i = 1; i < locations.Length + 1; i++)
        {
            spawnPosition = new Vector3((transform.position.x - (x * gridLength )), (transform.position.y - (y * gridLength)), (transform.position.z));

            if (printLogs)
                Debug.Log("spawnPosition: " + spawnPosition.ToString());

            // if the 
            if (locations[i-1] == 1)
            {
                // randomly get position to spawn asteroid 
                Instantiate(asteroidPrefabs[UnityEngine.Random.Range(0, asteroidPrefabs.Count)], spawnPosition, Random.rotation).GetComponent<Asteroid>();
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


}
