using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    /// <summary>
    /// Class used to transfer settings between the Menu scene and Level scenes.
    /// </summary>
    public class SettingsHolder
    {
        public static string currentVersion = "v5";

        public static float enemySpawnRate = 0.3f;
        public static float tramSpawnRate = 0.05f;

        /// <summary>
        /// Number between 0 and 1.
        /// </summary>
        public static float powerupDropChance = 0.9f;

        public static float zRange = 6.2f;
    }
}
