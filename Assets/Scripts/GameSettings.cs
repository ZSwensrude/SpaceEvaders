using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    private float asteroidSpeed;

    [SerializeField]
    private float asteroidSpawnInterval;

    [SerializeField]
    private int asteroidsInGroup;

    public float AsteroidSpeed { get => asteroidSpeed; set => asteroidSpeed = Mathf.Clamp(value, 0f, 1000f); }
    public float AsteroidSpawnInterval { get => asteroidSpawnInterval; set => asteroidSpawnInterval = Mathf.Clamp(value, 0f, 100f); }
    public int AsteroidsInGroup { get => asteroidsInGroup; set => asteroidsInGroup = Mathf.Clamp(value, 0, 9); }
}
