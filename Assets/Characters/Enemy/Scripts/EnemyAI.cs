using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : AbstractCharacter
{

    /// <summary>
    /// How many times per 1 second should enemy update his target.
    /// </summary>
    public float movementTargetUpdateRate = 1f;
    
    public int movementAngleRange = 30;

    /// <summary>
    /// Minimal distance from player.
    /// </summary>
    public float playerDetectionRange = 5f;

    public GameObject powerup1;
    public GameObject powerup2;

    BoxCollider2D boxCollider;

    Transform player;

    float nextMovementTargetUpdateTime;

    System.Random random;

    /// <summary>
    /// Checks if the player is near this enemy. If there's no player, returns false.
    /// </summary>
    /// <returns>True if the player is in playerDetectionRange of this character.</returns>
    public bool IsPlayerNear()
    {
        return player != null && Math.Abs((player.position - gameObject.transform.position).magnitude) <= playerDetectionRange;
    }

    protected override void OnDying()
    {
        SpawnPowerup();
    }

    protected override bool ShouldMove()
    {
        return true;
    }

    protected override void OnAfterMoved()
    {
        if (!IsPlayerNear() && IsTimeToUpdateTarget())
        {
            CalculateNewDirection();
            CalculateTimeToUpdateMovementTarget();
        }
    }

    private bool IsTimeToUpdateTarget()
    {
        return Time.time >= nextMovementTargetUpdateTime;
    }

    protected override void Init()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        nextMovementTargetUpdateTime = 0;
        random = new System.Random();
        movementDirection.x = 0;
        movementDirection.y = 1;
    }

    private void CalculateNewDirection()
    {
        float magnitude = movementDirection.magnitude;
        float currentAngle = 0;

        if (magnitude - 0 > 0.001)
        {
            // calculate current angle only if magnitude != 0
            currentAngle = (float)Math.Atan(movementDirection.y / movementDirection.x);
        } else
        {
            // set some magnitude if it's null
            magnitude = 1;
        }

        // convert it to radians
        currentAngle = (float)Math.PI * currentAngle / 180f;

        // new angle (in both directions) from given range
        float newAngle = random.Next(movementAngleRange) * (random.Next(2) == 0 ? 1 : -1);
        newAngle = (float)Math.PI * newAngle / 180f;

        // add the current one
        newAngle += currentAngle;

        // transform it back to vector
        float newX = (float)(magnitude * Math.Cos(newAngle)), 
            newY = (float)(magnitude * Math.Sin(newAngle));

        movementDirection.x = newX;
        movementDirection.y = newY;
        Debug.Log("Previous angle: " + currentAngle*180f/Math.PI);
        Debug.Log("New angle: " + newAngle * 180f / Math.PI);
        Debug.Log("New direction: " + movementDirection);
    }

    private void SpawnPowerup()
    {
        if (random.Next() <= SettingsHolder.powerupDropChance )
        {
            Instantiate(random.Next(2) == 0 ? powerup1 : powerup2, transform.position, transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj == null)
        {
            player = null;
        } else
        {
            player = playerObj.transform;
        }

        // if the player is near, follow him
        if (IsPlayerNear())
        {
            movementDirection = player.position - transform.position;
        }
        //Debug.Log(Time.time);
        //Debug.Log("Next:" +nextMovementTargetUpdateTime);
        //if (Time.time >= nextMovementTargetUpdateTime)
        //{
        //    Debug.Log("Move");
        //    player = playerObj.transform;
        //    CalculateNewDirection();
        //    //CalculateTimeToUpdateMovementTarget();
        //}

        //if (playerObj != null)
        //{
        //    // keep moving in player's direction
        //    player = playerObj.transform;
        //    movementDirection = player.position - transform.position;
        //}
    }

    private void CalculateTimeToUpdateMovementTarget()
    {
        nextMovementTargetUpdateTime = Time.time + 1f / movementTargetUpdateRate;
    }

    /// <summary>
    /// Returns true if there's no player or this enemy is far from player.
    /// </summary>
    /// <returns></returns>
    private bool IsFarFromPlayer()
    {
        return player == null || 
            !IsPlayerNear();
    }


    /// <summary>
    /// Draw are of the enemy's box colider.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerDetectionRange);
        Gizmos.DrawLine(transform.position, transform.position+(Vector3)movementDirection);
    }
}
