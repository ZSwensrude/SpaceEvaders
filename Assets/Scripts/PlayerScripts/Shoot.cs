using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public GameObject projectile;
    public float launchVel = 700f;
    public void ShootLaser(InputAction.CallbackContext context)
    {

        if (context.started)
        { 
            Vector3 laserPos = new Vector3(this.transform.position[0], this.transform.position[1], this.transform.position[2] + 3);
            GameObject laser = Instantiate(projectile, this.transform.position, this. transform.rotation);
        }

    }

    public void FixedUpdate() {
        
    }
}
