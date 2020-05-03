using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmazoPowerup : AbstractPowerup
{
    public uint hpRegen = 75;

    protected override void UsePowerup(PlayerControl player)
    {
        player.Heal(hpRegen);
    }
}
