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
        /// Name of the trigger for CigaPowerup animation.
        /// </summary>
        public const string CIGA_ANIM_TRIG = "ciga";

        /// <summary>
        /// Name of the trigger for SmazoPowerup animation.
        /// </summary>
        public const string SMAZO_ANIM_TRIG = "smazo";

        /// <summary>
        /// Controller used to display animation of picked up items.
        /// </summary>
        public Animator animator;

        public void TriggerAnimation(string triggerName)
        {
            animator.SetTrigger(triggerName);
        }

        /// <summary>
        /// Activates 'ciga' trigger.
        /// </summary>
        public void DisplayCiga()
        {
            animator.SetTrigger(CIGA_ANIM_TRIG);
        }

        public void DisplaySmazo()
        {
            animator.SetTrigger(SMAZO_ANIM_TRIG);
        }

        void Start()
        {

        }


    }
}
