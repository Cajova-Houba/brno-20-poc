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
        /// <summary>
        /// Returns the name of the scene for this level.
        /// </summary>
        /// <returns></returns>
        public abstract string GetLevelSceneName();
    }
}
