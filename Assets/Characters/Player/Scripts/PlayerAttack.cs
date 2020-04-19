using Assets.Characters.Player.Scripts;
using Assets.Scripts.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : AbstractPlayerAttack
{
    protected override bool ShouldAttack()
    {
        return true;
    }

}
