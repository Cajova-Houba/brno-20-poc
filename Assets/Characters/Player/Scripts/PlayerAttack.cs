using Assets.Scripts.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : AbstractAttack
{
    /// <summary>
    /// Layers with enemies, used during melee attack.
    /// </summary>
    public LayerMask enemyLayers;

    bool isAttacking = false;

    protected override bool ShouldAttack()
    {
        return isAttacking;
    }

    // Update is called once per frame
    void Update()
    {
        isAttacking = Input.GetKeyDown(KeyCode.J);
    }

    protected override void Attack()
    {
        // detect hit enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // damage enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit " + enemy.name);
            EnemyAI enemyAI = enemy.GetComponentInParent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(damage);
            }

            // attack only 1 enemy
            break;

        }
    }
}
