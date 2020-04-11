using Assets.Scripts;
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

    public bool spawningEnabled = true;

    /// <summary>
    /// If turned of, spawnRate value will be used.
    /// If turned on, value from SettingsHolder will be used.
    /// </summary>
    public bool useSettings = true;

    protected float nextSpawnTime = 0;

    protected GameObject[] spawnPoints;

    public float GetUsedSpawnRate()
    {
        return useSettings ? GetSettingsSpawnRate() : spawnRate;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        InitializeSpawnPointsByTag();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawningEnabled && IsTimeToSpawn())
        {
            GameObject spawnPoint = GetSpawnPoint();
            if (spawnPoint != null)
            {
                GameObject objectToSpawn = GetGameObjectToSpawn(spawnPoint);
                Debug.Log("Spawning: " + spawnPoint.transform.position + " ; " + spawnPoint.transform.rotation);
                GameObject newObject = Instantiate(objectToSpawn, spawnPoint.transform.position, spawnPoint.transform.rotation);
                PostSpawnAction(newObject);
            }

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
    protected virtual void PostSpawnAction(GameObject spawnedObject)
    {

    }

    protected bool IsSpawnRateZero()
    {
        return Math.Abs(spawnRate - 0) < 0.001;
    }

    protected virtual bool IsTimeToSpawn()
    {
        // spawnRate == 0 => don't spawn anything
        return !IsSpawnRateZero() && Time.time > nextSpawnTime;
    }

    protected void CalculateSpawnTime()
    {
        float rate = GetUsedSpawnRate();

        nextSpawnTime = Time.time + 1f / rate;
    }

    /// <summary>
    /// Get value from SettingsHolder that should be used by this spawner.
    /// </summary>
    /// <returns></returns>
    protected abstract float GetSettingsSpawnRate();

    /// <summary>
    /// Implementation specific initialization. Called after spawn points
    /// are initialized.
    /// </summary>
    protected abstract void Init();

    protected abstract GameObject GetGameObjectToSpawn(GameObject spawnPoint);

    protected abstract GameObject GetSpawnPoint();

    protected abstract string GetSpawnPointTag();
}
