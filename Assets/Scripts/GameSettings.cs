using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    [Header("Debug")]
    [SerializeField]
    private bool printLogs;

    [Header("Asteroids")]
    [SerializeField]
    private float asteroidSpeed;

    [SerializeField]
    private float asteroidSpawnInterval;

    [SerializeField]
    private int asteroidsInGroup;

    [Header("Player Behaviour")]
    [SerializeField]
    private float boostSpeed;

    [SerializeField]
    private float moveDistance;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float gridLength;

    [Header("Score Settings")]

    [SerializeField]
    private bool incrementScore;

    [Header("Boost Values")]
    
    [SerializeField]
    private int boostRegen;
    
    [SerializeField]
    private int boostUsage;


    public bool PrintLogs { get => printLogs; set => printLogs = value; }
    public float AsteroidSpeed { get => asteroidSpeed; set => asteroidSpeed = Mathf.Clamp(value, 0f, 1000f); }
    public float AsteroidSpawnInterval { get => asteroidSpawnInterval; set => asteroidSpawnInterval = Mathf.Clamp(value, 0f, 100f); }
    public int AsteroidsInGroup { get => asteroidsInGroup; set => asteroidsInGroup = Mathf.Clamp(value, 0, 9); }
    public float MoveDistance { get => moveDistance; set => moveDistance = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float BoostSpeed { get => boostSpeed; set => boostSpeed = value; }
    public float GridLength { get => gridLength; set => gridLength = value; }
    public bool IncrementScore { get => incrementScore; set => incrementScore = value; }
    public int BoostRegen { get => boostRegen; set => boostRegen = value; }
    public int BoostUsage { get => boostUsage; set => boostUsage = value; }
}
