using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalinatorSpawner : AbstractSpawner
{
    public GameObject salina;

    System.Random random;

    protected override void Init()
    {
        random = new System.Random();
    }

    protected override GameObject GetGameObjectToSpawn()
    {
        return salina;
    }

    protected override Transform GetSpawnPoint()
    {
        return spawnPoints[random.Next(spawnPoints.Length)].transform;
    }

    protected override string GetSpawnPointTag()
    {
        return "SalinaSpawnPoint";
    }
}
