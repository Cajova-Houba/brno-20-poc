using System;
using System.Collections;
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

        /// <summary>
        /// Animator used to send triggers to play animations.
        /// </summary>
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

        /// <summary>
        /// Cooldown.
        /// </summary>
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
        /// Coroutine that uses this attack. Plays the animation, yields, actually 
        /// executes the attack logic, waits for the animation to finish and re-sets the cooldown.
        /// 
        /// The attack is split into two parts so that the actual damage is done in the middle of the animation
        /// which is sort of the point where the player/enemy touches its target.
        /// </summary>
        public IEnumerator UseAttack()
        {
            CalculateNextTimeToAttack();
            PlayAttackAnimation();
            yield return new WaitForSeconds(GetTimeToStartAttacking());
            Attack();
            yield return new WaitForSeconds(GetTimeToFinishAttackAnimation());
        }


        /// <summary>
        /// Returns the time needed to finish the animation afther the actual Attack() is 
        /// executed.
        /// </summary>
        /// <returns></returns>
        protected float GetTimeToFinishAttackAnimation()
        {
            return animationSecDuration / 2.0f;
        }

        /// <summary>
        /// Returns the time after the attack animation start when Attack() should be executed.
        /// </summary>
        /// <returns></returns>
        protected float GetTimeToStartAttacking()
        {
            return animationSecDuration / 2.0f;
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
        

        public void PlayAttackAnimation()
        {
            if (animator != null)
            {
                animator.SetTrigger("attacking");
            }
        }

        /// <summary>
        /// Sets next cooldown.
        /// </summary>
        protected void CalculateNextTimeToAttack()
        {
            nextTimeToAttack = Time.time + 1f / attackRate;
        }

        /// <summary>
        /// Checks whether the cooldown has ticked off.
        /// </summary>
        /// <returns></returns>
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
