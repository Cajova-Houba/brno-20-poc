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

        public Level1 level1;

        public Barrier[] barriers;

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
            int lState = level1.GetLevelState();

            if (barriers == null || lState < 0 || barriers.Length <= lState)
            {
                return;
            }

            if (barriers[lState].enemyLimit <= killCount)
            {
                Debug.Log("Disabling barrier: " + lState);
                barriers[lState].gameObject.SetActive(false);
                level1.IncrementLevelState();
            }
        }
    }
}
