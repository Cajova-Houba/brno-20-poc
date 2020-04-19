using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Levels
{
    /// <summary>
    /// One phase of Level 1 used to hold initialy spawned enemies.
    /// </summary>
    public class Level1Phase : MonoBehaviour
    {
        /// <summary>
        /// Enemies that are initialy playced into the game map. Deactivated by default 
        /// and activated once this phase is activated.
        /// </summary>
        public GameObject[] initialEnemies;

        /// <summary>
        /// Number of the phase as described in doc of Level1.
        /// </summary>
        public int phaseNumber;

        /// <summary>
        /// Activates all enemies in this phase.
        /// </summary>
        public void ActivatePhase()
        {
            foreach (GameObject initialEnemy in initialEnemies)
            {
                initialEnemy.SetActive(true);
            }
        }

        private void Start()
        {
            foreach (GameObject initialEnemy in initialEnemies)
            {
                initialEnemy.SetActive(false);
            }
        }
    }
}
