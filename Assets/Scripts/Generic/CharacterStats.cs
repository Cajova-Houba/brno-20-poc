using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Generic
{
    /// <summary>
    /// Simple class that holds character stats such as HP or energy.
    /// </summary>
    public class CharacterStats
    {
        public int Hp { get; set; }

        public int Energy { get; set; }

        public bool IsDead()
        {
            return Hp <= 0;
        }

        public void Heal(int hp, int maxHP)
        {
            Hp = Math.Min(maxHP, Hp + hp);
        }

        public void HealEnergy(int energy, int maxEnergy)
        {
            Energy = Math.Min(maxEnergy, Energy + energy);
        }

        public void UseEnergy(int energy)
        {
            Energy = Math.Max(0, Energy - energy);
        }

        /// <summary>
        /// Checks if the character has enough energy.
        /// </summary>
        /// <param name="requiredEnergy">Energy required by some action.</param>
        /// <returns>True if currentEnergy >= requiredEnergy</returns>
        public bool HasEnoughEnergy(int requiredEnergy)
        {
            return Energy >= requiredEnergy;
        }
    }
}
