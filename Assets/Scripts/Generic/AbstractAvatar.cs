using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Generic
{
    /// <summary>
    /// Abstract class for components displaying health and energy.
    /// </summary>
    public abstract class AbstractAvatar : MonoBehaviour
    {
        public abstract void SetMaxHealth(uint maxHealth);

        public abstract void SetHealth(uint health);

        public abstract void SetMaxEnergy(uint maxEnergy);

        public abstract void SetEnergy(uint energy);
    }
}
