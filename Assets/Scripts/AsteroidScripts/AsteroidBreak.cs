using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBreak : MonoBehaviour
{
    [SerializeField]
    private GameObject asteroidShattered;

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(asteroidShattered, transform.position, transform.rotation);
        GetComponent<Rigidbody>().AddExplosionForce(2, transform.position, 50);
        Destroy(gameObject);
    }
}
