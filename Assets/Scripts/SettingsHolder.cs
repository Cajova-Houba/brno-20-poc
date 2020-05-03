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
        public static string currentVersion = "v10";

        public static float enemySpawnRate = 0.7f;
        public static float tramSpawnRate = 0.05f;

        /// <summary>
        /// Number between 0 and 1.
        /// </summary>
        public static float powerupDropChance = 0.8f;

        public static float zRange = 6.2f;

        public static string playerTagName = "PlayerTag";

        public static string deadPlayerTagName = "DeadPlayerTag";
    }
}
