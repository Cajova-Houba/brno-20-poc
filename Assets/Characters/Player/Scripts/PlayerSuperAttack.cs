using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Characters.Player.Scripts
{
    public class PlayerSuperAttack : AbstractPlayerAttack
    {
        protected override bool ShouldAttack()
        {
            return playerControl.HasEnoughEnergy(requiredEnergy);
        }

        protected override string GetTriggerName()
        {
            return "superAttack";
        }
    }
}
