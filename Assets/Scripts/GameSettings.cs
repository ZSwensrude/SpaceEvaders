using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    private float asteroidSpeed;

    public float AsteroidSpeed { get => asteroidSpeed; set => asteroidSpeed = Mathf.Clamp(value, 0f, 1000f); }
}
