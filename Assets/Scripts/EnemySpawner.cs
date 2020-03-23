using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public Transform player;

    public GameObject kozaak;
    public GameObject husaak;
    public GameObject prasaak;

    public int enemyTypeCount = 3;

    GameObject[] spawnPoints;
    
    /// <summary>
    /// Spawn one enemy every 1 second.
    /// </summary>
    public float spawnRate = 1;

    float nextSpawnTime = 0;

    System.Random random;


    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        random = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsTimeToSpawn())
        {
            Transform spawnPoint = PickRandomSpawnPoint();
            GameObject enemy = PickRandomEnemyToSpawn();
            Debug.Log("Spawning: "+spawnPoint.position + " ; " + spawnPoint.rotation);
            Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);

            CalculateSpawnTime();
        }

    }

    private GameObject PickRandomEnemyToSpawn()
    {
        int r = random.Next(enemyTypeCount);
        switch(r)
        {
            case 0: return husaak;
            case 1: return kozaak;
            case 2: return prasaak;
            default: return husaak;

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
        // spawnRate == 0 => don't spawn anything
        return Math.Abs(spawnRate - 0) > 0.001 && Time.time > nextSpawnTime;
    }
}
