using Assets.Scripts.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DefaultHealthBar : AbstractAvatar
{
    public Slider slider;

    public override void SetMaxHealth(uint maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public override void SetHealth(uint health)
    {
        slider.value = health;
    }

    public override void SetMaxEnergy(uint maxEnergy)
    {
    }

    public override void SetEnergy(uint energy)
    {
    }
}
