using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private GameSettings gameSettings;

    private float speed;
    
    private float despawnZ = -15f;

    private Vector3 targetPosition;
    private float step;


    private void Awake()
    {
        Debug.Log("asteroid spawned!");
        targetPosition = new Vector3(transform.position.x, transform.position.y, despawnZ);
        speed = gameSettings.AsteroidSpeed;
        step = speed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        if (transform.position.z == despawnZ)
        {
            Destroy(gameObject);
        }
    }

}