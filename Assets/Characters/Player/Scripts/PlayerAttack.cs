using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackPoint;

    /// <summary>
    /// Attack range around attack point.
    /// </summary>
    public float meleeAttackRange = 0.5f;

    /// <summary>
    /// Layers with enemies, used during melee attack.
    /// </summary>
    public LayerMask enemyLayers;

    /// <summary>
    /// Number of attacks per 1 second. 
    /// </summary>
    public float meleeAttackRate = 2f;
    float nextAttackTime = 0f;

    /// <summary>
    /// Damage this player does.
    /// </summary>
    public int damage = 25;

    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        bool isAttacking = Input.GetKeyDown(KeyCode.LeftControl);

        if (isAttacking)
        {
            MeleeAttack();
        }
    }

    private void MeleeAttack()
    {
        if (!IsTimeToAttack())
        {
            return;
        }

        // play melee attack animation
        // todo
        Debug.Log("Attacking");
        animator.SetTrigger("attacking");

        // detect hit enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, meleeAttackRange, enemyLayers);

        // damage enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit " + enemy.name);
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(damage);
            }

            // attack only 1 enemy
            break;

        }

        CalculateNextAttackTime();
    }

    private void CalculateNextAttackTime()
    {
        nextAttackTime = Time.time + 1f / meleeAttackRate;
    }

    private bool IsTimeToAttack()
    {
        return Time.time >= nextAttackTime;
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
