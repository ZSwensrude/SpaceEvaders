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


    private List<Asteroid> asteroids = new List<Asteroid>();

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

            yield return new WaitForSeconds(spawnInterval);
        }
    }


    private void SpawnAsteroids (int num)
    {
        for (int i = 0; i < num; i++)
        {
            Asteroid asteroid = Instantiate(asteroidPrefabs[0], transform.position, Quaternion.identity).GetComponent<Asteroid>();
            asteroids.Add(asteroid);
        }
    }


}
