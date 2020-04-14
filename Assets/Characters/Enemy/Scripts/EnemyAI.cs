using Assets.Scripts;
using Assets.Scripts.Generic;
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

    /// <summary>
    /// Array of rays used to avoid obastacles.
    /// </summary>
    Vector3[] rayPoints = new Vector3[PATH_RAY_COUNT];

    /// <summary>
    /// Flags for rayPoints array, pointing to ray paths where no collisions were detected. 
    /// false = collision detected.
    /// </summary>
    bool[] availableRayPaths = new bool[PATH_RAY_COUNT];

    System.Random random;

    /// <summary>
    /// Attack to be used. Reset in coroutine after it's used.
    /// </summary>
    AbstractAttack nextAttackToUse;

    /// <summary>
    /// Flag set when attack starts and re-set when the attack ends. When the attacking
    /// flag is set, no new attack can be picked.
    /// </summary>
    bool attacking = false;

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
        if (IsAttacking())
        {
            // enemies do not move when they are attacking
            return 0;
        }
        else if (IsPlayerNear())
        {
            // not attacking but following player
            return playerFollowMovementSpeed;
        }
        else
        {
            // wandering
            return base.GetMovementSpeed();
        }
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

    /// <summary>
    /// Calculates rays to be used to detect collisions in front of this enemy and checks if there's
    /// a collision for each ray.
    /// 
    /// Results are stored in rayPoints and availableRayPaths array.
    /// </summary>
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

            // calculate collision
            availableRayPaths[i] = !Physics2D.Linecast(stiffBody.transform.position, stiffBody.transform.position + rayPoints[i], 1 << 13);   
        }
    }

    private void CalculateNewDirection()
    {
        // pick ray which does not collide with anything
        List<Vector3> availablePaths = new List<Vector3>();
        for(int i = 0; i < PATH_RAY_COUNT; i++)
        {
            if (availableRayPaths[i])
            {
                availablePaths.Add(rayPoints[i]);
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

    /// <summary>
    /// Returns true if the nextAttack object is set.
    /// </summary>
    /// <returns></returns>
    private bool IsAttacking()
    {
        return attacking;
    }

    private bool IsTimeToUpdateTarget()
    {
        return Time.time >= nextMovementTargetUpdateTime;
    }

    private void SpawnPowerup()
    {
        if (random.NextDouble() <= SettingsHolder.powerupDropChance )
        {
            // todo: create function y -> z somewhere and use it
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.y - 6.2f);
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

        // if the player is near, follow him and try to attack
        if (IsPlayerNear())
        {
            movementDirection = player.position - transform.position;
            if (!attacking)
            {
                if (nextAttackToUse == null)
                {
                    // player is near, I'm not attacking and no attack is selected -> select one
                    Debug.Log(name + " is selecting attack.");
                    PickAttackToUse();
                } else
                {
                    // player is near, I'm not attacking and attack is selected -> use it
                    Debug.Log(name + " is attacking.");
                    StartCoroutine(UseAndResetAttack());
                }
            }
        }
    }

    /// <summary>
    /// Coroutine which uses the attack, waits for the attack animation to finish and 
    /// then re-sets the nextAttack object.
    /// This way, enemy starts moving again only after the attack is finished.
    /// </summary>
    /// <returns></returns>
    private IEnumerator UseAndResetAttack()
    {
        attacking = true;
        nextAttackToUse.UseAttack();
        yield return new WaitForSeconds(nextAttackToUse.animationSecDuration);
        Debug.Log("Attacking coroutine finished.");
        nextAttackToUse = null;
        attacking = false;
    }

    /// <summary>
    /// Method goes through all of character's attacks and picks the first usable attack.
    /// </summary>
    private void PickAttackToUse()
    {
        if (attacks == null)
        {
            return;
        }

        foreach(AbstractAttack attack in attacks)
        {
            if (attack.CanUseAttack() && attack.IsInAttackingRange())
            {
                nextAttackToUse = attack;
                break;
            }
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
            Vector3 endPoint = stiffBody.transform.position + ray;
            // check rays for possible collisions 
            for(int i = 0; i < PATH_RAY_COUNT; i++)
            {
                if (availableRayPaths[i])
                {
                    Gizmos.color = Color.white;
                } else
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawLine(stiffBody.transform.position, endPoint);
            }
        }

        Gizmos.color = Color.green;
        Gizmos.DrawLine(stiffBody.transform.position, stiffBody.transform.position + (Vector3)movementDirection);

    }
}
