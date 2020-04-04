using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmazoPowerup : AbstractPowerup
{
    public int hpRegen = 20;

    protected override void UsePowerup(PlayerControl player)
    {
        player.Heal(hpRegen);
    }
}
