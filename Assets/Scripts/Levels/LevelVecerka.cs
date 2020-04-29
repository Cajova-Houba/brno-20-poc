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

        public override string GetLevelSceneName()
        {
            return "Vecerka";
        }

        public override int GetEnemiesKilled()
        {
            return 0;
        }

        void Start()
        {
            CharacterStats stats = PlayerDataStore.RetrievePlayerData();
            if (stats == null)
            {
                Debug.Log("No player data in PlayerDataStore.");
            } else
            {
                player.SetCharacterStats(stats);
            }
        }

        void Update()
        {
            if (player != null)
            {
                if (player.GetKilledEnemiesCount() > 0)
                {
                    youWonDialog.Activate();
                }
            } else
            {
                Debug.Log("No player object in level Vecerka.");
            }
        }
    }
}
