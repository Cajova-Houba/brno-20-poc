using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public Transform player;

    public GameObject enemyPrefab;

    GameObject[] spawnPoints;
    
    /// <summary>
    /// Spawn one enemy every 1 second.
    /// </summary>
    public float spawnRate = 1;

    float nextSpawnTime = 0;


    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    // Update is called once per frame
    void Update()
    {
        if (IsTimeToSpawn())
        {
            Transform spawnPoint = PickRandomSpawnPoint();
            Debug.Log("Spawning: "+spawnPoint.position + " ; " + spawnPoint.rotation);
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            CalculateSpawnTime();
        }

    }

    private Transform PickRandomSpawnPoint()
    {
        return spawnPoints[0].transform;
    }

    private void CalculateSpawnTime()
    {
        nextSpawnTime = Time.time + 1f / spawnRate;
    }

    private bool IsTimeToSpawn()
    {
        return Time.time > nextSpawnTime;
    }
}
