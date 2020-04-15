using Assets.Scripts.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : AbstractAttack
{
    public LayerMask playerLayer;

    /// <summary>
    /// Reference to AI controlling this object.
    /// </summary>
    public EnemyAI ai;

    // Start is called before the first frame update
    void Start()
    {

    }

    public override bool IsInAttackingRange()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        return hitEnemies.Length > 0;
    }

    protected override bool ShouldAttack()
    {
        // attack only if the player is near
        return ai.IsPlayerNear();
    }

    protected override int Attack()
    {
        // detect hit enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        // damage enemies
        foreach (Collider2D playerCol in hitEnemies)
        {
            Debug.Log("Hiting " + playerCol.name);
            PlayerControl player = playerCol.GetComponentInParent<PlayerControl>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            // attack only 1 player
            break;

        }

        return 0;
    }
}
