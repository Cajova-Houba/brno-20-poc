using Assets.Scripts;
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

    protected override void Init()
    {
        random = new System.Random();
    }

    protected override GameObject GetGameObjectToSpawn(GameObject spawnPoint)
    {
        EnemySpawnPoint esp = spawnPoint.GetComponent<EnemySpawnPoint>();
        return esp.GetEnemyToSpawn();
    }

    protected override GameObject GetSpawnPoint()
    {
        List<GameObject> enemySpawnPoints = new List<GameObject>();
        foreach(GameObject sp in spawnPoints)
        {
            EnemySpawnPoint esp = sp.GetComponent<EnemySpawnPoint>();
            if (esp != null && esp.IsPlayerNear())
            {
                enemySpawnPoints.Add(sp);
            }
        }

        if (enemySpawnPoints.Count == 0)
        {
            return null;
        } else
        {
            return enemySpawnPoints[random.Next(enemySpawnPoints.Count)];
        }
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
