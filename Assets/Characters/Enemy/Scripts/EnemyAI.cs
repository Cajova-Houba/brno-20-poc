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

    public bool isPassive = false;

    /// <summary>
    /// Minimal distance from one of the players target points. Enemy will stop moving after reaching this 
    /// distance from the target point.
    /// </summary>
    public float playerTargetPointMinDistance = 0.05f;

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

    /// <summary>
    /// Returns true if one of the player's target points was reached.
    /// </summary>
    /// <returns></returns>
    protected bool IsNextToPlayer()
    {
        if (player == null)
        {
            return false;
        }

        Vector2 frontT, backT;
        GetPlayerTargetDirections(out frontT, out backT);

        //Debug.Log("fornt distance: " + frontT.magnitude);
        //Debug.Log("back distance: " + backT.magnitude);
        return Math.Abs(frontT.magnitude) < playerTargetPointMinDistance 
            || Math.Abs(backT.magnitude) < playerTargetPointMinDistance;
    }

    protected override Transform GetTarget()
    {
        if (IsPlayerNear())
        {
            return player.transform;
        } else
        {
            return base.GetTarget();
        }
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
        if (IsAttacking() || IsStunned())
        {
            // enemies do not move when they are attacking or are stunned
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
    protected override void Update()
    {
        base.Update();

        GameObject playerObj = GameObject.FindGameObjectWithTag(SettingsHolder.playerTagName);
        CalculateRayPoints();

        if (playerObj == null)
        {
            Debug.Log("No game object with tag " + SettingsHolder.playerTagName + " found.");
            player = null;
        } else
        {
            player = playerObj.transform;
        }

        // if the player is near, follow him and try to attack
        if (IsPlayerNear() && !isPassive)
        {
            Debug.Log(name + " is near player.");
            //movementDirection = player.position - transform.position;
            MoveToPlayerTarget();
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
    /// Picks the nearest of the player's target points and sets the moving direction accordingly.
    /// By using the target points, enemies will not follow the player himself, but rather one of 
    /// these target points from which they can attack him comfortably.
    /// </summary>
    /// <returns></returns>
    private void MoveToPlayerTarget()
    {
        if (player == null || player.GetComponent<PlayerControl>() == null)
        {
            Debug.Log("Valid player object not set.");
        }
        else
        {
            Vector2 frontT, backT;
            GetPlayerTargetDirections(out frontT, out backT);

            // pick the one that is nearer this enemy
            if (frontT.sqrMagnitude <= backT.sqrMagnitude)
            {
                movementDirection = frontT;
            } else
            {
                movementDirection = backT;
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
        yield return StartCoroutine(nextAttackToUse.UseAttack(this));
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
            if (attack.CanUseAttack())
            {
                //nextAttackToUse = attack;
                //break;
                if (attack.IsInAttackingRange())
                {
                    nextAttackToUse = attack;
                    break;
                }
                else
                {
                    Debug.Log(name + " can use attack " + attack.name + " but the attack is not in range.");
                }
            }
            else
            {
                Debug.Log(name + " can't use attack " + attack.name);
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
    /// Sets the directions to player's front and back target point from this enemy's position.
    /// </summary>
    /// <param name="frontVector">Vector to the front target.</param>
    /// <param name="backVector">Vector to the back target.</param>
    private void GetPlayerTargetDirections(out Vector2 frontVector, out Vector2 backVector)
    {
        if (player == null || player.GetComponent<PlayerControl>() == null)
        {
            Debug.Log("No valid player object.");
            frontVector = new Vector2(0,0);
            backVector = new Vector2(0,0);
        } else
        {
            PlayerControl pc = player.GetComponent<PlayerControl>();
            frontVector = pc.frontTargetPoint.transform.position - stiffBody.transform.position;
            backVector = pc.backTargetPoint.transform.position - stiffBody.transform.position;
        }
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
