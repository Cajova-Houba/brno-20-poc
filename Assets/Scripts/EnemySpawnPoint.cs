using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    /// <summary>
    /// Range used to detect whether player is nearby or not.
    /// </summary>
    public int spawnPointRange = 10;

    /// <summary>
    /// When the spawn points is active and is asked to get enemy to spawn, random double is generated
    /// and if it's > than this number, spawning is skipped.
    /// 
    /// If this number is > 1, then there's also chance of spawning multiple enemies.
    /// </summary>
    public float spawnRateModifier = 1f;

    public LayerMask playerLayer;

    /// <summary>
    /// Enemies this spawn point can spawn.
    /// </summary>
    public GameObject[] spawnableEnemies;

    private System.Random random;

    /// <summary>
    /// Checks if the player is in range of this spawn point.
    /// </summary>
    /// <returns></returns>
    public bool IsPlayerNear()
    {
        Collider2D[] playerColisions = Physics2D.OverlapCircleAll(transform.position, spawnPointRange, playerLayer);
        return playerColisions.Length > 0;
    }

    public GameObject[] GetEnemiesToSpawn()
    {
        if (random.NextDouble() <= spawnRateModifier)
        {
            if (spawnRateModifier > 1 && random.NextDouble() <= (spawnRateModifier - 1f))
            {
                return new GameObject[] {
                    spawnableEnemies[random.Next(spawnableEnemies.Length)],
                    spawnableEnemies[random.Next(spawnableEnemies.Length)]
                };
            } else
            {
                return new GameObject[]
                {
                    spawnableEnemies[random.Next(spawnableEnemies.Length)]
                };
            }
        } else
        {
            return null;
        }
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


    /// <summary>
    /// Draw small circle around attack point - good for debugging.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, spawnPointRange);
    }
}
