using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fire : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Vector3 laserOffset = new Vector3(0f, 0f, 3f);
    [SerializeField] private int laserMax = 10;
    [SerializeField] private AudioSource laserSound;

    public void FireLaser(InputAction.CallbackContext context)
    {
        if(_projectilePrefab.GetComponent<Laser>().LaserNum <= laserMax && context.started)
        {
            laserSound.Play();
            Instantiate(_projectilePrefab, transform.position + laserOffset, _projectilePrefab.transform.rotation);
        }
    }
}