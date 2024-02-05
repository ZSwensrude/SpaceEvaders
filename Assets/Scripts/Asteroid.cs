using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    /*
    [SerializeField]
    private List<GameObject> asteroids;

    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float despawnZ = 0f;

    private GameObject asteroid;
    */

    private void Awake()
    {
        SpawnAsteroid();
    }

    private void SpawnAsteroid ()
    {
        Debug.Log("asteroid spawned!");
        //asteroid = Instantiate(asteroids[0], transform.position, Quaternion.identity);
    }

}
