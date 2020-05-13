using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Class representing picked up powerup. It's a child of player object and can be activated. Upon activation, an animation is played and the object
    /// is hidden after the animation ends.
    /// </summary>
    public class PickedUpPowerup : MonoBehaviour
    {
        /// <summary>
        /// Controller used to display animation of picked up items.
        /// </summary>
        public Animator animator;

        public void TriggerAnimation(string triggerName)
        {
            animator.SetTrigger(triggerName);
        }


    }
}
