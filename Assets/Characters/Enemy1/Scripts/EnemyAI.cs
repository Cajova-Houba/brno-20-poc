using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
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

    /// <summary>
    /// Max HP of this enemy.
    /// </summary>
    public int maxHP = 100;

    public HealthBar healthBar;

    private int currentHP;

    Rigidbody2D rb;
    BoxCollider2D boxCollider;

    Transform player;

    Vector2 playerDirection;

    bool facingRight;

    float nextMovementTargetUpdateTime;
    
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        healthBar.SetHealth(currentHP);
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Dying");
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        healthBar.SetMaxHealth(currentHP);
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        facingRight = true;
        nextMovementTargetUpdateTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
            playerDirection = player.position - transform.position;

            Flip();
        }
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
        return player != null && (playerDirection).magnitude > movementRange;
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
