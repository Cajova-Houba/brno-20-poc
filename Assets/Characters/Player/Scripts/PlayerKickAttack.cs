using Assets.Scripts.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Characters.Player.Scripts
{
    public class PlayerKickAttack : AbstractPlayerAttack
    {

        protected override string GetTriggerName()
        {
            return "kickAttack";
        }

        protected override bool ShouldAttack()
        {
            return playerControl.HasEnoughEnergy(requiredEnergy);
        }
    }
}
