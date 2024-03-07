using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBreak : MonoBehaviour
{
    [SerializeField]
    private GameObject asteroidShattered;

    private void OnTriggerEnter(Collider other)
    {
        // if on projectile layer
        if (other.gameObject.layer == 3)
        {
            Instantiate(asteroidShattered, transform.position, Random.rotation).GetComponent<Transform>();
            Instantiate(asteroidShattered, transform.position, Random.rotation).GetComponent<Transform>();
            Destroy(gameObject);
        }
    }
}
