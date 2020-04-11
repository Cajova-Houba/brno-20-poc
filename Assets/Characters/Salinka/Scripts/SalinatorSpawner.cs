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

    protected override GameObject GetGameObjectToSpawn(GameObject spawnPoint)
    {
        return salina;
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
