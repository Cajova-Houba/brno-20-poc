using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    /// <summary>
    /// Enemies this spawn point can spawn.
    /// </summary>
    public GameObject[] spawnableEnemies;

    private System.Random random;

    public GameObject SpawnEnemy()
    {
        return spawnableEnemies[random.Next(spawnableEnemies.Length)];
    }

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
