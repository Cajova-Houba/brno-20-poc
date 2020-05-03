using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Generic
{
    /// <summary>
    /// Simple class that holds character stats such as HP or energy.
    /// </summary>
    public class CharacterStats
    {
        public uint Hp { get; set; }

        public uint Energy { get; set; }

        public bool IsDead()
        {
            return Hp <= 0;
        }

        public void Heal(uint hp, uint maxHP)
        {
            Hp = Math.Min(maxHP, Hp + hp);
        }

        public void HealEnergy(uint energy, uint maxEnergy)
        {
            Energy = Math.Min(maxEnergy, Energy + energy);
        }

        public void UseEnergy(uint energy)
        {
            Debug.Log("Using " + energy + " energy");
            Energy = Math.Max(0, Energy - energy);
            Debug.Log("Remaining energy: " + Energy);
        }

        /// <summary>
        /// Substracts given amount of damage from HP (capped at 0).
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(uint damage)
        {
            if (damage > Hp)
            {
                Hp = 0;
            } else
            {
                Hp = Hp - damage;
            }
        }

        /// <summary>
        /// Sets the HP to 0.
        /// </summary>
        public void Die()
        {
            Hp = 0;
        }

        /// <summary>
        /// Checks if the character has enough energy.
        /// </summary>
        /// <param name="requiredEnergy">Energy required by some action.</param>
        /// <returns>True if currentEnergy >= requiredEnergy</returns>
        public bool HasEnoughEnergy(uint requiredEnergy)
        {
            return Energy >= requiredEnergy;
        }
    }
}
