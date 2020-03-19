using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    /// <summary>
    /// How many times per 1 second should enemy update his target.
    /// </summary>
    public float movementTargetUpdateRate = 2f;

    public float movementSpeed = 3f;

    /// <summary>
    /// Minimal distance from player.
    /// </summary>
    public float movementRange = 5f;

    Rigidbody2D rb;
    BoxCollider2D boxCollider;

    Transform player;

    Vector2 playerDirection;

    bool facingRight;

    float nextMovementTargetUpdateTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        facingRight = true;
        nextMovementTargetUpdateTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        playerDirection = player.position - transform.position;

        Flip();
    }

    void FixedUpdate()
    {
        if (IsFarFromPlayer())
        {
            rb.MovePosition(rb.position + playerDirection.normalized * movementSpeed * Time.fixedDeltaTime);
            CalculateTimeToUpdateMovementTarget();
        } else
        {
            // don't move if we're near the player
            rb.MovePosition(rb.position);
        }
    }

    private void CalculateTimeToUpdateMovementTarget()
    {
        nextMovementTargetUpdateTime = Time.time + 1f / movementTargetUpdateRate;
    }

    private bool IsFarFromPlayer()
    {
        Debug.Log(playerDirection.magnitude);
        return (playerDirection).magnitude > movementRange;
    }

    void Flip()
    {
        if ((playerDirection.x < 0 && facingRight)
            || (playerDirection.x > 0 && !facingRight))
        {
            facingRight = !facingRight;
            rb.transform.Rotate(new Vector3(0, 180, 0));
        }
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
