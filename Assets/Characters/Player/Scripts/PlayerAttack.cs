using Assets.Scripts.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : AbstractAttack
{
    public override bool IsInAttackingRange()
    {
        return true;
    }

    protected override bool ShouldAttack()
    {
        return true;
    }

    protected override int Attack()
    {
        // detect hit enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, targetLayer);

        // damage all enemies
        int deadEnemies = 0;
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit " + enemy.name);
            EnemyAI enemyAI = enemy.GetComponentInParent<EnemyAI>();
            if (enemyAI != null)
            {
                if(enemyAI.TakeDamage(damage))
                {
                    deadEnemies++;
                }
            }
        }

        return deadEnemies;
    }

}
