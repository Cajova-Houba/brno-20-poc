using Assets.Scripts;
using Assets.Scripts.Levels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawn points are dividied into 4 stages on the map as follows:
/// 
/// 0: 2 spawn points, easy enemies
/// 1: 2 spawn points, normal enemies
/// 2: 2 spawn poitns, normal enemies
/// 3: 2 spawn points, hard enemies
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    public const int MAX_STAGE = 3;

    System.Random random;

    public EnemySpawnPoint[] stage0;
    public EnemySpawnPoint[] stage1;
    public EnemySpawnPoint[] stage2;
    public EnemySpawnPoint[] stage3;

    /// <summary>
    /// Number of the current stage, starts at 0.
    /// </summary>
    int currentStage = 0;

    /// <summary>
    /// Spawns given number of enemies at spawn points in the current stage.
    /// </summary>
    /// <param name="enemyCount">Number of enemies to spawn.</param>
    public void SpawnInCurrentStage(int enemyCount)
    {
        Debug.Log("Spawning " + enemyCount + " enemies in stage " + currentStage);
        EnemySpawnPoint[] spawnPointsToUse;
        switch(currentStage)
        {
            case 0:
                spawnPointsToUse = stage0;
                break;
            case 1:
                spawnPointsToUse = stage1;
                break;
            case 2:
                spawnPointsToUse = stage2;
                break;
            case 3:
            default:
                spawnPointsToUse = stage3;
                break;
        }

        UseSpawnPoints(spawnPointsToUse, enemyCount);
    }
    
    /// <summary>
    /// Increments the current stage up to the MAX_STAGE.
    /// </summary>

    public void IncrementStage()
    {
        if (currentStage == MAX_STAGE)
        {
            return;
        }

        Debug.Log("Incrementing spawn stage " + currentStage);
        currentStage++;
    }


    private void Start()
    {

    }

    private void Update()
    {
        random = new System.Random();
    }

    /// <summary>
    /// Randomly spawns given number of enemies on given spawn points.
    /// </summary>
    /// <param name="spawnPoints">Spawn points to use.</param>
    /// <param name="enemiesToSpawn">Number of enemies to spawn.</param>
    private void UseSpawnPoints(EnemySpawnPoint[] spawnPoints, int enemiesToSpawn)
    {
        int spCount = spawnPoints.Length;
        for(int i = 0; i < enemiesToSpawn; i++)
        {
            EnemySpawnPoint spawnPoint = spawnPoints[random.Next(spCount)];
            GameObject objectToSpawn = spawnPoint.SpawnEnemy();
            Debug.Log("Spawning: " + objectToSpawn.name + " at "+spawnPoint.gameObject.name);
            GameObject newObject = Instantiate(objectToSpawn, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
    }
}
 