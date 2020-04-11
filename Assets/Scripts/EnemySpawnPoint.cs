using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    /// <summary>
    /// Range used to detect whether player is nearby or not.
    /// </summary>
    public int spawnPointRange = 10;

    public LayerMask playerLayer;

    /// <summary>
    /// Checks if the player is in range of this spawn point.
    /// </summary>
    /// <returns></returns>
    public bool IsPlayerNear()
    {
        Collider2D[] playerColisions = Physics2D.OverlapCircleAll(transform.position, spawnPointRange, playerLayer);
        return playerColisions.Length > 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
