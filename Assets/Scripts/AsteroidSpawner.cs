using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{

    [SerializeField]
    private GameSettings gameSettings;
    [SerializeField]
    private List<GameObject> asteroidPrefabs;

    // used in script, also used to set default values
    [SerializeField]
    private float spawnInterval = 10f;
    [SerializeField]
    private int numToSpawn = 1;
    [SerializeField]
    private float asteroidSpeed = 10f;

    private void Awake()
    {
        // reset all the scriptable object values to default
        gameSettings.AsteroidSpeed = asteroidSpeed;
        gameSettings.AsteroidSpawnInterval = spawnInterval;
        gameSettings.AsteroidsInGroup = numToSpawn;
    }

    void Start()
    {
        Debug.Log("AsteroidSpawner Start");
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop ()
    {
        while (true)
        {
            // get values from scriptable object
            asteroidSpeed = gameSettings.AsteroidSpeed;
            spawnInterval = gameSettings.AsteroidSpawnInterval;
            numToSpawn = gameSettings.AsteroidsInGroup;
    
            // spawn asteroids
            SpawnAsteroids(numToSpawn);

            // increase speed of next asteroids (proof that we can change the speed while playing)
            //gameSettings.AsteroidSpeed = asteroidSpeed + 10f;

            // wait for the interval before running again
            yield return new WaitForSeconds(spawnInterval);
        }
    }


    private void SpawnAsteroids (int num)
    {
        for (int i = 0; i < num; i++)
        {
            // randomly get position to spawn asteroid 
            Instantiate(asteroidPrefabs[UnityEngine.Random.Range(0, asteroidPrefabs.Count)], transform.position, Quaternion.identity).GetComponent<Asteroid>();
        }
    }


}
