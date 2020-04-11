using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : AbstractCharacter
{
    public const int PATH_RAY_COUNT = 10;

    /// <summary>
    /// How many times per 1 second should enemy update his target.
    /// </summary>
    public float movementTargetUpdateRate = 1f;
    
    public int movementAngleRange = 30;

    /// <summary>
    /// Minimal distance from player.
    /// </summary>
    public float playerDetectionRange = 5f;

    public float playerFollowMovementSpeed = 2.5f;

    /// <summary>
    /// Powerups dropped by this enemy when he dies.
    /// </summary>
    public GameObject[] powerups;

    BoxCollider2D boxCollider;

    Transform player;

    float nextMovementTargetUpdateTime;

    Vector3[] rayPoints = new Vector3[PATH_RAY_COUNT];

    System.Random random;

    /// <summary>
    /// Checks if the player is near this enemy. If there's no player, returns false.
    /// </summary>
    /// <returns>True if the player is in playerDetectionRange of this character.</returns>
    public bool IsPlayerNear()
    {
        return player != null && Math.Abs((player.position - gameObject.transform.position).magnitude) <= playerDetectionRange;
    }

    protected bool IsNextToPlayer()
    {
        return player != null && Math.Abs((player.position - gameObject.transform.position).magnitude) <= 1.5f;
    }

    protected override void OnDying()
    {
        SpawnPowerup();
    }

    protected override bool ShouldMove()
    {
        // wander around if the player is dead,
        // or if he isn't dead and you're not next to him
        return player == null || !IsNextToPlayer();
    }

    protected override void OnAfterMoved()
    {
        if (!IsPlayerNear() && IsTimeToUpdateTarget())
        {
            CalculateNewDirection();
            CalculateTimeToUpdateMovementTarget();
        }
    }

    protected override float GetMovementSpeed()
    {
        if (IsPlayerNear())
        {
            return playerFollowMovementSpeed;
        } else
        {
            return base.GetMovementSpeed();
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

        // since enemies are spawned at the top, set
        // initial movement direction to bottom
        movementDirection.x = 0;
        movementDirection.y = -1;
    }

    private void CalculateRayPoints()
    { 
        float baseAngle = (float)Math.Atan2(movementDirection.y, movementDirection.x) * 180 / (float)Math.PI;
        int angleStep = movementAngleRange / PATH_RAY_COUNT;
        float halfRange = movementAngleRange / 2;
        float magnitude = 1.5f;
        for (int i = 0; i < PATH_RAY_COUNT; i++)
        {
            float newAngle = (baseAngle + halfRange - i * angleStep);
            float newX = (float)(magnitude * Math.Cos(newAngle * Math.PI / 180)),
            newY = (float)(magnitude * Math.Sin(newAngle * Math.PI / 180));

            rayPoints[i] = new Vector3(newX, newY, 0);
        }
    }

    private void CalculateNewDirection()
    {
        // pick ray which does not collide with anything
        List<Vector3> availablePaths = new List<Vector3>();
        foreach(Vector3 pathRay in rayPoints)
        {
            bool isColision = false;
            RaycastHit2D[] rayHits = Physics2D.LinecastAll(stiffBody.transform.position, stiffBody.transform.position + pathRay, 1 << 13);
            isColision = rayHits.Length > 0;

            if (!isColision)
            {
                availablePaths.Add(pathRay);
            }
        }

        //Debug.Log("Available paths: " + availablePaths.Count);
        if (availablePaths.Count == 0)
        {
            // no available paths, stay still
            movementDirection.x = 0;
            movementDirection.y = 0;
        } else
        {
            int randomPathIndex = random.Next(availablePaths.Count);
            movementDirection.x = availablePaths[randomPathIndex].x;
            movementDirection.y = availablePaths[randomPathIndex].y;
        }

        //Debug.Log("New direction: " + movementDirection);
    }

    private void SpawnPowerup()
    {
        if (random.NextDouble() <= SettingsHolder.powerupDropChance )
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, sprite.transform.position.z);
            Instantiate(powerups[random.Next(powerups.Length)], pos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        CalculateRayPoints();

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
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRange);

        // draw all rays in movement angle
        foreach(Vector3 ray in rayPoints)
        {
            // todo: linecast is already done once, use the result and do not do it again here
            Vector3 endPoint = stiffBody.transform.position + ray;
            // check rays for possible collisions 
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(ray, 0.3f);
            RaycastHit2D[] rayHits = Physics2D.LinecastAll(stiffBody.transform.position, endPoint, 1 << 13);
            foreach(RaycastHit2D rayHit in rayHits)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(stiffBody.transform.position, rayHit.point);
            }

            if (rayHits.Length == 0)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(stiffBody.transform.position, endPoint);
            }
            Gizmos.DrawWireSphere(endPoint, 0.3f);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawLine(stiffBody.transform.position, stiffBody.transform.position + (Vector3)movementDirection);

    }
}
