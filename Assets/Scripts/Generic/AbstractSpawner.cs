using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSpawner : MonoBehaviour
{
    /// <summary>
    /// How many spawns should be done in 1 second.
    /// </summary>
    public float spawnRate = 1;

    protected float nextSpawnTime = 0;

    protected GameObject[] spawnPoints;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        InitializeSpawnPointsByTag();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsTimeToSpawn())
        {
            Transform spawnPoint = GetSpawnPoint();
            GameObject objectToSpawn = GetGameObjectToSpawn();
            Debug.Log("Spawning: " + spawnPoint.position + " ; " + spawnPoint.rotation);
            GameObject newObject = Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
            PostSpawnAction(newObject);

            CalculateSpawnTime();
        }
    }

    protected void InitializeSpawnPointsByTag()
    {
        string tag = GetSpawnPointTag();
        spawnPoints = GameObject.FindGameObjectsWithTag(tag);
    }

    /// <summary>
    /// Called afther the object is instantiated.
    /// </summary>
    /// <param name="spawnedObject">Instantiated object.</param>
    protected void PostSpawnAction(GameObject spawnedObject)
    {

    }

    protected bool IsSpawnRateZero()
    {
        return Math.Abs(spawnRate - 0) < 0.001;
    }

    protected bool IsTimeToSpawn()
    {
        // spawnRate == 0 => don't spawn anything
        return !IsSpawnRateZero() && Time.time > nextSpawnTime;
    }

    protected void CalculateSpawnTime()
    {
        nextSpawnTime = Time.time + 1f / spawnRate;
    }

    /// <summary>
    /// Implementation specific initialization. Called after spawn points
    /// are initialized.
    /// </summary>
    protected abstract void Init();

    protected abstract GameObject GetGameObjectToSpawn();

    protected abstract Transform GetSpawnPoint();

    protected abstract string GetSpawnPointTag();
}
