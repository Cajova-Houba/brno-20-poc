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
        /// Currently only 3 states in level 1
        /// then the level ends
        /// </summary>
        private int level1State = 0;

        void Update()
        {
            if (player != null)
            {
                int killCount = player.GetKilledEnemiesCount();
                TryUnlockNextBarrier(killCount);
            }
        }

        /// <summary>
        /// If the killCount is high enough, increments the level state and unlocks next barrier.
        /// </summary>
        /// <param name="killCount"></param>
        private void TryUnlockNextBarrier(int killCount)
        {
            if (level1State == 3)
            {
                return;
            }

            if (barriers == null || barriers.Length != 3)
            {
                return;
            }

            if (barriers[level1State].enemyLimit <= killCount)
            {
                barriers[level1State].gameObject.SetActive(false);
                level1State++;
            }
        }
    }
}
