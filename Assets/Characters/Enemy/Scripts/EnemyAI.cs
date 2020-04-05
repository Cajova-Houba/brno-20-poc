using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : AbstractCharacter
{

    /// <summary>
    /// How many times per 1 second should enemy update his target.
    /// </summary>
    public float movementTargetUpdateRate = 2f;

    /// <summary>
    /// Minimal distance from player.
    /// </summary>
    public float movementRange = 5f;

    public GameObject powerup1;
    public GameObject powerup2;

    BoxCollider2D boxCollider;

    Transform player;

    float nextMovementTargetUpdateTime;

    System.Random random;
    


    protected override void OnDying()
    {
        SpawnPowerup();
    }

    protected override bool ShouldMove()
    {
        return IsFarFromPlayer();
    }

    protected override void OnAfterMoved()
    {
        CalculateTimeToUpdateMovementTarget();
    }

    protected override void Init()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        nextMovementTargetUpdateTime = 0;
        random = new System.Random();
    }

    private void SpawnPowerup()
    {
        if (random.Next(2) != 0 )
        {
            Instantiate(random.Next(2) == 0 ? powerup1 : powerup2, transform.position, transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            // keep moving in player's direction
            player = playerObj.transform;
            movementDirection = player.position - transform.position;
        }
    }

    private void CalculateTimeToUpdateMovementTarget()
    {
        nextMovementTargetUpdateTime = Time.time + 1f / movementTargetUpdateRate;
    }

    private bool IsFarFromPlayer()
    {
        return player != null && (movementDirection).magnitude > movementRange;
    }


    /// <summary>
    /// Draw are of the enemy's box colider.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (boxCollider == null)
        {
            return;
        }

        Gizmos.DrawWireCube(boxCollider.transform.position, boxCollider.offset);
    }
}
