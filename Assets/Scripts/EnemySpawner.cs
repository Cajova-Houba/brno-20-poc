using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : AbstractSpawner
{
    public GameObject kozaak;
    public GameObject husaak;
    public GameObject prasaak;

    public int enemyTypeCount = 3;

    System.Random random;

    protected override void Init()
    {
        random = new System.Random();
    }

    protected override GameObject GetGameObjectToSpawn()
    {
        int r = random.Next(enemyTypeCount);
        switch (r)
        {
            case 0: return husaak;
            case 1: return kozaak;
            case 2: return prasaak;
            default: return husaak;

        }
    }

    protected override Transform GetSpawnPoint()
    {
        return spawnPoints[0].transform;
    }

    protected override string GetSpawnPointTag()
    {
        return "SpawnPoint";
    }
}
