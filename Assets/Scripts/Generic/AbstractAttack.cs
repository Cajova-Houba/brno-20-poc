using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Generic
{
    public abstract class AbstractAttack : MonoBehaviour
    {
        public Transform attackPoint;

        /// <summary>
        /// Attack range around attack point.
        /// </summary>
        public float attackRange = 0.5f;

        public Animator animator;

        public int damage = 30;

        /// <summary>
        /// Number of attacks per 1 second. 
        /// </summary>
        public float attackRate = 2f;

        /// <summary>
        /// Energy required to perform this attack.
        /// </summary>
        public int requiredEnergy = 0;

        protected float nextTimeToAttack = 0f;

        /// <summary>
        /// Additional conditions for attacking provided by implementing class.
        /// </summary>
        /// <returns></returns>
        protected abstract bool ShouldAttack();

        /// <summary>
        /// Attack logic.
        /// </summary>
        protected abstract void Attack();

        protected void FixedUpdate() {
            if (IsTimeToAttack() && ShouldAttack())
            {
                Debug.Log(name+" is attacking");
                PlayAttackAnimation();
                Attack();
                CalculateNextTimeToAttack();
            }
        }

        protected void PlayAttackAnimation()
        {
            Debug.Log("Playing attack animation");
            if (animator != null)
            {
                animator.SetTrigger("attacking");
            }
        }

        protected void CalculateNextTimeToAttack()
        {
            nextTimeToAttack = Time.time + 1f / attackRate;
        }

        protected bool IsTimeToAttack()
        {
            return Time.time >= nextTimeToAttack;
        }

        /// <summary>
        /// Draw small circle around attack point - good for debugging.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null)
            {
                return;
            }

            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }

    }
}
