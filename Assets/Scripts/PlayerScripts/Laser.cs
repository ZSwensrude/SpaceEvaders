using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField] private float velocity = 20f;
    [SerializeField] private static int laserNum = 0;
    [SerializeField] private float despawnZ = 50f;

    public int LaserNum { get{ return laserNum; } }
    public void Start()
    {
        laserNum++;
        this.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, velocity);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
        Destroy(gameObject);
        laserNum--;
    }

    public void FixedUpdate() {
        if(transform.position.z >= despawnZ) {
            Destroy(gameObject);
            laserNum--;
        }
    }

}