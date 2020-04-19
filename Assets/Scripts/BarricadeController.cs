using Assets.Scripts.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class BarricadeController : MonoBehaviour
    {
        public PlayerControl player;

        public Barrier[] barriers;

        /// <summary>
        /// Index to barriers marking the currently active barrier.
        /// </summary>
        private int activeBarrier;

        /// <summary>
        /// If the killCount is high enough, increments the level state and unlocks next barrier.
        /// </summary>
        /// <param name="killCount"></param>
        /// <returns>True if the barrier was unlocked.</returns>
        public bool TryUnlockNextBarrier(int killCount)
        {
            if (barriers == null || activeBarrier < 0 || barriers.Length <= activeBarrier)
            {
                Debug.Log("Cant unlock next barrier.");
                return false;
            }

            if (barriers[activeBarrier].enemyLimit <= killCount)
            {
                Debug.Log("Disabling barrier: " + activeBarrier);
                barriers[activeBarrier].gameObject.SetActive(false);
                activeBarrier++;
                return true;
            }

            return false;
        }

        private void Start()
        {
            activeBarrier = 0;
        }
    }
}
