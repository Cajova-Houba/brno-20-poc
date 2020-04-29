using Assets.Scripts.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Levels
{
    /// <summary>
    /// Class used to transfer player data bewtween scenes.
    /// </summary>
    public class PlayerDataStore
    {
        private static CharacterStats playerStats;

        /// <summary>
        /// Stores given player data.
        /// </summary>
        public static void StorePlayerData(CharacterStats data)
        {
            playerStats = data;
        }

        public static CharacterStats RetrievePlayerData()
        {
            return playerStats;
        }
    }
}
