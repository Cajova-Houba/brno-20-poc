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
        /// <summary>
        /// Layer which is targetted by this attack.
        /// </summary>
        public LayerMask targetLayer;

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

        /// <summary>
        /// Duration of the attack animation. Also the duration for which the character won't move.
        /// </summary>
        public float animationSecDuration = 1f;

        protected float nextTimeToAttack = 0f;

        /// <summary>
        /// Checks if there are any objects in range of the attack point from given layer.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsInAttackingRange();

        /// <summary>
        /// Checks if the attack cooldown has passed and any additional conditions in ShouldAttack() are fulfilled.
        /// 
        /// This method returning true means this attack can be used by calling the UseAttackMethod().
        /// </summary>
        /// <returns></returns>
        public bool CanUseAttack()
        {
            return IsTimeToAttack() && ShouldAttack();
        }

        /// <summary>
        /// Uses this attack. Plays the animations and re-sets the cooldown.
        /// </summary>
        public void UseAttack()
        {
            PlayAttackAnimation();
            Attack();
            CalculateNextTimeToAttack();
        }

        /// <summary>
        /// Additional conditions for attacking provided by implementing class.
        /// </summary>
        /// <returns></returns>
        protected abstract bool ShouldAttack();

        /// <summary>
        /// Attack logic.
        /// </summary>
        protected abstract void Attack();
        

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
