using Assets.Scripts.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Levels
{
    public class LevelVecerka : AbstractLevel
    {
        public YouWonDialog youWonDialog;

        public override void OnIntroEnd()
        {
            Time.timeScale = 1f;
        }

        public override string GetLevelSceneName()
        {
            return "Vecerka";
        }

        public override int GetEnemiesKilled()
        {
            return 0;
        }

        protected override void Start()
        {
            base.Start();
            CharacterStats stats = PlayerDataStore.RetrievePlayerData();
            if (stats == null)
            {
                Debug.Log("No player data in PlayerDataStore.");
            } else
            {
                player.SetCharacterStats(stats);
            }

            // turned on by intro
            Time.timeScale = 0f;
        }

        protected override void HandleLevelUpdate()
        {
            if (player != null)
            {
                if (player.GetKilledEnemiesCount() > 0)
                {
                    youWonDialog.Activate();
                }
            }
            else
            {
                Debug.Log("No player object in level Vecerka.");
            }
        }
    }
}
