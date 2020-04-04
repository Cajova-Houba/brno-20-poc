using Assets.Scripts.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Characters.Player.Scripts
{
    public class PlayerKickAttack : AbstractAttack
    {
        /// <summary>
        /// Layers with enemies, used during melee attack.
        /// </summary>
        public LayerMask enemyLayers;

        /// <summary>
        /// Player control object used to consume energy.
        /// </summary>
        public PlayerControl playerControl;

        bool isAttacking = false;

        protected override bool ShouldAttack()
        {
            return isAttacking && playerControl.HasEnoughEnergy(requiredEnergy);
        }


        protected override void Attack()
        {
            // use energy, even though noone might be hit by attack
            playerControl.UseEnergy(requiredEnergy);

            // detect hit enemies
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

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
        }
        void Start()
        {
            requiredEnergy = 20;
        }

        // Update is called once per frame
        void Update()
        {
            isAttacking = Input.GetKeyDown(KeyCode.K);
        }
    }
}
