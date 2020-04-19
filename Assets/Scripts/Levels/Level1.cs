using Assets.Scripts.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Levels
{
    /// <summary>
    /// Contains logic for the frist level. Level 1 has couple of phases, new phase is activated
    /// by killing all enemies in the current phase. The phases are :
    /// 
    /// 0: Only one enemy, no spawns
    /// 
    /// 1: 2 initial easy enemies
    /// 2: 2 easy enemies are spawned
    /// 
    /// 3: 3 initial normal enemies
    /// 4: 2 normal enemies are spawned.
    /// 
    /// 5: 1 normal enemy
    /// 6: 2 normal enemies spawned
    /// 7: 2 normal enemies spawned
    /// 8: 2 normal enemies spawned
    /// 9: 1 normal enemy spawned
    /// 
    /// 10: 2 initial hard enemies
    /// 11: 2 hard enemies spawned
    /// 12: 1 hard enemy spawned
    /// 13: 1 hard enemy spawned
    /// 14: 3 hard enemies spawned
    /// 15: 1 hard enemy spawned
    /// 
    /// 16: end of level
    /// 
    /// </summary>
    public class Level1 : AbstractLevel
    {
        /// <summary>
        /// Number of the last phase. Once it's reached, the level ends.
        /// </summary>
        public const int LAST_PHASE = 16;

        public PlayerControl player;

        public BarricadeController barricadeController;

        public EnemySpawner enemySpawner;

        /// <summary>
        /// Phases with initial enemies.
        /// </summary>
        public Level1Phase[] phases;

        /// <summary>
        /// Total number of enemies killed by player in this level.
        /// </summary>
        int enemiesKilled = 0;

        /// <summary>
        /// Numeric representation of the current pahse of the level. Starts at 0.
        /// </summary>
        int levelPhase = 0;

        /// <summary>
        /// Total number of enemies to be killed to move to the next phase.
        /// Index is the number of the current phase.
        /// </summary>
        int[] killsForNextPhase = new int[] { 1, 3, 5, 8, 10, 11, 13, 15, 17, 18, 20, 22, 23, 24, 27, 28 };

        /// <summary>
        /// Increments the level phase counter, removes possible barries and spawns enemies for the new phase, if necessary.
        /// </summary>
        public void NextLevelPhase()
        {
            Debug.Log("Incrementing level phase " + levelPhase);
            levelPhase ++;
            barricadeController.TryUnlockNextBarrier(enemiesKilled);
            ActivateCurrentPhase();
            SpawnEnemiesForCurrentPhase();
        }

        public int GetLevelState()
        {
            return levelPhase;
        }
        public override string GetLevelSceneName()
        {
            return "Level1";
        }

        /// <summary>
        /// Increments the spawner stage if the current phase is high enough and spawns enemies.
        /// This method should be called afther the phase is incremented.
        /// </summary>
        private void SpawnEnemiesForCurrentPhase()
        {
            Debug.Log("Spawning enemies for phase " + levelPhase);
            switch (levelPhase)
            {
                case 3:
                case 5:
                case 10:
                    enemySpawner.IncrementStage();
                    break;

                case 9:
                case 12:
                case 13:
                case 15:
                    enemySpawner.SpawnInCurrentStage(1);
                    break;

                case 2:
                case 4:
                case 6:
                case 7:
                case 8:
                case 11:
                    enemySpawner.SpawnInCurrentStage(2);
                    break;

                case 14:
                    enemySpawner.SpawnInCurrentStage(3);
                    break;
            }
        }

        void Start()
        {
            levelPhase = 0;
            enemiesKilled = 0;
            
            // activate phase 0
            ActivateCurrentPhase();
        }

        void Update()
        {
            if (player != null)
            {
                enemiesKilled = player.GetKilledEnemiesCount();
                TryIncrementPhase();
            }
        }

        /// <summary>
        /// Tries to move to the next phase based on the number of enemies killed.
        /// </summary>
        private void TryIncrementPhase()
        {
            if (levelPhase == LAST_PHASE)
            {
                return;
            }

            if (enemiesKilled == killsForNextPhase[levelPhase])
            {
                NextLevelPhase();
            }
        }

        /// <summary>
        /// Activates phase with phaseNumber equal to levelPhase.
        /// </summary>
        private void ActivateCurrentPhase()
        {
            foreach(Level1Phase phase in phases)
            {
                if (phase.phaseNumber == levelPhase)
                {
                    phase.ActivatePhase();
                    break;
                }
            }
        }

    }
}
