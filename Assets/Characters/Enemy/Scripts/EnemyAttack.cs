using Assets.Scripts.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : AbstractAttack
{
    public LayerMask playerLayer;

    // Start is called before the first frame update
    void Start()
    {

    }

    protected override bool ShouldAttack()
    {
        // no other conditions
        return true;
    }

    protected override void Attack()
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
    }
}
