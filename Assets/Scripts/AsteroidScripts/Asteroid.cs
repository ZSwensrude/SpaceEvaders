using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private bool printLogs;

    [SerializeField]
    private GameSettings gameSettings;

    private float speed;
    
    private float despawnZ = -15f;

    private Vector3 targetPosition;
    private float step;


    private void Awake()
    {
        printLogs = gameSettings.PrintLogs;

        if (printLogs)
            Debug.Log("asteroid spawned!");
        targetPosition = new Vector3(transform.position.x, transform.position.y, despawnZ);
    }

    private void FixedUpdate()
    {
        speed = gameSettings.AsteroidSpeed;
        step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        if (transform.position.z == despawnZ)
        {
            Destroy(gameObject);
        }
    }

}
