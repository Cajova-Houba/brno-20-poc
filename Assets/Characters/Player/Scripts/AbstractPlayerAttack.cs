using Assets.Scripts.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Characters.Player.Scripts
{
    public abstract class AbstractPlayerAttack : AbstractAttack
    {
        /// <summary>
        /// Player control object used to consume energy.
        /// </summary>
        public PlayerControl playerControl;

        private List<AudioSource> attackSounds;

        private System.Random random = new System.Random();

        public override bool IsInAttackingRange()
        {
            return true;
        }

        protected override int Attack()
        {
            // use energy, even though noone might be hit by attack
            playerControl.UseEnergy(requiredEnergy);

            // detect hit enemies
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, targetLayer);

            // play sound if at least one enemy was hit
            if (hitEnemies.Length > 0)
            {
                PlayAttackSound();
            }

            // damage all enemies
            int deadEnemies = 0;
            foreach (Collider2D enemy in hitEnemies)
            {
                EnemyAI enemyAI = enemy.GetComponentInParent<EnemyAI>();
                if (enemyAI != null)
                {
                    Debug.Log("Hit " + enemy.name);
                    if (enemyAI.TakeDamage(damage))
                    {
                        deadEnemies++;
                    }
                }
            }

            return deadEnemies;
        }

        void Start()
        {
            // get attack sounds
            GameObject[] soundObjects = GameObject.FindGameObjectsWithTag("PlayerAttackSound");
            attackSounds = new List<AudioSource>();
            foreach(GameObject soundObject in soundObjects)
            {
                AudioSource audio = soundObject.GetComponent<AudioSource>();
                if (audio != null)
                {
                    attackSounds.Add(audio);
                }
            }
        }

        /// <summary>
        /// Randomly plays one attack sound.
        /// </summary>
        private void PlayAttackSound()
        {
            if (attackSounds != null && attackSounds.Count > 0)
            {
                attackSounds[random.Next(attackSounds.Count)].Play();
            }
        }
    }
}
