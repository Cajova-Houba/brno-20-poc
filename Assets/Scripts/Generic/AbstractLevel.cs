using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Generic
{
    public abstract class AbstractLevel : MonoBehaviour
    {
        public PlayerControl player;

        /// <summary>
        /// Returns the name of the scene for this level.
        /// </summary>
        /// <returns></returns>
        public abstract string GetLevelSceneName();

        /// <summary>
        /// Returns the number of enemies killed by player in this level so far.
        /// </summary>
        /// <returns></returns>
        public abstract int GetEnemiesKilled();
    }
}
