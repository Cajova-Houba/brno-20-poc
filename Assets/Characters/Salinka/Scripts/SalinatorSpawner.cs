using Assets.Scripts;
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

    protected override GameObject[] GetGameObjectsToSpawn(GameObject spawnPoint)
    {
        return new GameObject[] { salina };
    }

    protected override GameObject GetSpawnPoint()
    {
        return spawnPoints[random.Next(spawnPoints.Length)];
    }

    protected override string GetSpawnPointTag()
    {
        return "SalinaSpawnPoint";
    }

    protected override float GetSettingsSpawnRate()
    {
        return SettingsHolder.tramSpawnRate;
    }
}
