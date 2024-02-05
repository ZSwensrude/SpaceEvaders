using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] 
    private float spawnInterval = 10f;
    [SerializeField]
    private int numToSpawn = 1;

    [SerializeField]
    private List<GameObject> asteroidPrefabs;

    [SerializeField]
    private GameSettings gameSettings;

    private float asteroidSpeed;

    void Start()
    {
        Debug.Log("AsteroidSpawner Start");
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop ()
    {
        while (true)
        {
            Debug.Log("trying to spawn asteroid");
            SpawnAsteroids(numToSpawn);

            asteroidSpeed = gameSettings.AsteroidSpeed;

            gameSettings.AsteroidSpeed = asteroidSpeed + 10f;

            yield return new WaitForSeconds(spawnInterval);
        }
    }


    private void SpawnAsteroids (int num)
    {
        for (int i = 0; i < num; i++)
        {
            Instantiate(asteroidPrefabs[0], transform.position, Quaternion.identity).GetComponent<Asteroid>();
        }
    }


}
