using Assets.Scripts;
using Assets.Scripts.Levels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Iterates through its spawn points and picks the active ones. Then randomly chooses one 
/// of the active spawnpoints and spawn the enemy.
/// </summary>
public class EnemySpawner : AbstractSpawner
{
    System.Random random;

    public Level1 level1;

    public EnemySpawnPoint[] lowSpawnPoints;
    public EnemySpawnPoint[] midSpawnPoints;
    public EnemySpawnPoint[] highSpawnPoints;

    protected override void Init()
    {
        random = new System.Random();
    }

    protected override GameObject[] GetGameObjectsToSpawn(GameObject spawnPoint)
    {
        EnemySpawnPoint esp = spawnPoint.GetComponent<EnemySpawnPoint>();
        return esp.SpawnEnemy();
    }

    protected override GameObject GetSpawnPoint()
    {
        switch(level1.GetLevelState())
        {
            case 0:
                return UseOneSpawnPoint(lowSpawnPoints);
            case 1:
                return UseOneSpawnPoint(midSpawnPoints);
            case 2:
                return UseOneSpawnPoint(highSpawnPoints);
            default:
                if (level1.GetLevelState() > 2)
                {
                    return UseOneSpawnPoint(highSpawnPoints);
                } else
                {
                    return null;
                }
        }
    }

    /// <summary>
    /// Randomly picks one spawn point near to player from given set and returns it.
    /// </summary>
    /// <param name="spawnPoints"></param>
    /// <returns>Spawn point or null if no spawn point is avalable.</returns>
    private GameObject UseOneSpawnPoint(EnemySpawnPoint[] spawnPoints)
    {
        List<EnemySpawnPoint> spawnPointsNearPlayer = new List<EnemySpawnPoint>();
        foreach (EnemySpawnPoint sp in spawnPoints)
        {
            if (sp.IsPlayerNear())
            {
                spawnPointsNearPlayer.Add(sp);
            }
        }

        int spCount = spawnPointsNearPlayer.Count;
        if (spCount == 0)
        {
            return null;
        }

        return spawnPointsNearPlayer[random.Next(spCount)].gameObject;
    }

    protected override string GetSpawnPointTag()
    {
        return "SpawnPoint";
    }

    protected override float GetSettingsSpawnRate()
    {
        return SettingsHolder.enemySpawnRate;
    }
}
