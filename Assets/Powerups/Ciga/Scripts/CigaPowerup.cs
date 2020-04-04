using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Powerups.Smazo.Scripts
{
    public class CigaPowerup : AbstractPowerup
    {
        public int energyRegen = 20;

        protected override void UsePowerup(PlayerControl player)
        {
            player.HealEnergy(energyRegen);
        }
    }
}
