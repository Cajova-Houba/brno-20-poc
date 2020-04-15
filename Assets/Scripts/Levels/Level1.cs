using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Levels
{
    /// <summary>
    /// Contains logic for the frist level.
    /// </summary>
    public class Level1 : MonoBehaviour
    {
        public PlayerControl player;

        int levelState = 0;

        public void IncrementLevelState()
        {
            levelState ++;
        }

        public int GetLevelState()
        {
            return levelState;
        }

        void Start()
        {
            levelState = 0;
        }
        
    }
}
