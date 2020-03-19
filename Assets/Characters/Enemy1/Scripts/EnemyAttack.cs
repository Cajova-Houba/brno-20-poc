using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform attackPoint;

    public LayerMask playerLayer;

    /// <summary>
    /// Number of attacks per second.
    /// </summary>
    public float attackRate = 1f;

    public int damage = 30;

    /// <summary>
    /// Attack range around attack point.
    /// </summary>
    public float meleeAttackRange = 0.5f;

    float nextTimeToAttack = 0f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsTimeToAttack())
        {
            Attack();
            CalculateNewTimeToAttack();
        }
    }

    private void CalculateNewTimeToAttack()
    {
        nextTimeToAttack = Time.time + 1f / attackRate;
    }

    private void Attack()
    {
        // play melee attack animation
        // todo
        Debug.Log("Enemy " + name + " is attacking");

        // detect hit enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, meleeAttackRange, playerLayer);

        // damage enemies
        foreach (Collider2D playerCol in hitEnemies)
        {
            Debug.Log("Hiting " + playerCol.name);
            PlayerControl player = playerCol.GetComponent<PlayerControl>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            // attack only 1 player
            break;

        }
    }

    private bool IsTimeToAttack()
    {
        return Time.time >= nextTimeToAttack;
    }

    /// <summary>
    /// Draw small circle around attack point - good for debugging.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, meleeAttackRange);
    }
}
