using Assets.Scripts.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Characters.Player.Scripts
{
    /// <summary>
    /// Class used to control avatar that displays HP and energy.
    /// 
    /// HP animation is split into 14 frames. That means each frame represents
    /// maxHealth/14 hit points.
    /// </summary>
    public class PlayerAvatar : AbstractAvatar
    {
        /// <summary>
        /// 14 frames for HP, 1 (the 15th) for no HP.
        /// </summary>
        public const uint HP_ANIMATION_FRAME_COUNT = 14;

        /// <summary>
        /// 14 frames for energy, 1 (the 14th) for no energy.
        /// </summary>
        public const uint ENERGY_ANIMATION_FRAME_COUNT = 14;

        /// <summary>
        /// Image component that displays HP.
        /// </summary>
        public Image hpImage;

        /// <summary>
        /// Image component that displays energy.
        /// </summary>
        public Image energyImage;

        public Sprite[] hpSprites;

        public Sprite[] energySprites;

        private uint maxHealth, maxEnergy;
        private uint healthPerFrame, energyPerFrame;
        private uint health, energy;



        public override void SetMaxHealth(uint maxHealth)
        {
            this.maxHealth = maxHealth;
            healthPerFrame = maxHealth / HP_ANIMATION_FRAME_COUNT;
        }

        public override void SetHealth(uint health)
        {
            Debug.Log("Setting HP to " + health);
            this.health = health;
            UpdateHpImage();
        }

        public override void SetMaxEnergy(uint maxEnergy)
        {
            this.maxEnergy = maxEnergy;
            energyPerFrame = maxEnergy / ENERGY_ANIMATION_FRAME_COUNT;
            Debug.Log("Energy per frame: " + energyPerFrame);
        }

        public override void SetEnergy(uint energy)
        {
            Debug.Log("Setting energy to " + energy);
            this.energy = energy;
            UpdateEnergyImage();
        }

        /// <summary>
        /// Updates energy image component to show the current energy.
        /// </summary>
        private void UpdateEnergyImage()
        {
            if (energyImage == null)
            {
                Debug.Log("No energy image component.");
                return;
            }
            else if (energySprites == null)
            {
                Debug.Log("No energy sprites.");
                return;
            }
            else if (energySprites.Length != HP_ANIMATION_FRAME_COUNT + 1)
            {
                Debug.Log("Wrong number of sprites: " + energySprites.Length);
                return;
            }

            uint frameIndex = GetEnergyFrameIndex();
            Debug.Log("Setting energy to frame " + frameIndex);
            energyImage.sprite = energySprites[frameIndex];
        }

        /// <summary>
        /// Converts current energy to frame index using ANIMATION_FRAME_COUNT.
        /// </summary>
        /// <returns>0 based frame index. Max value is ANIMATION_FRAME_COUNT.</returns>
        private uint GetEnergyFrameIndex()
        {
            return Math.Min(ENERGY_ANIMATION_FRAME_COUNT, ENERGY_ANIMATION_FRAME_COUNT - (uint)Math.Ceiling((float)energy / energyPerFrame));
        }

        /// <summary>
        /// Updates HP image component to show the current health.
        /// </summary>
        private void UpdateHpImage()
        {
            if (hpImage == null)
            {
                Debug.Log("No HP image component.");
                return;
            } else  if (hpSprites == null)
            {
                Debug.Log("No HP sprites.");
                return;
            } else if (hpSprites.Length != HP_ANIMATION_FRAME_COUNT+1)
            {
                Debug.Log("Wrong number of sprites: " + hpSprites.Length);
                return;
            }

            uint frameIndex = GetHpFrameIndex();
            Debug.Log("Setting HP to frame " + frameIndex);
            hpImage.sprite = hpSprites[frameIndex];
        }

        /// <summary>
        /// Converts current HP to frame index using ANIMATION_FRAME_COUNT.
        /// </summary>
        /// <returns>0 based frame index. Max value is ANIMATION_FRAME_COUNT.</returns>
        private uint GetHpFrameIndex()
        {
            return Math.Min(HP_ANIMATION_FRAME_COUNT,HP_ANIMATION_FRAME_COUNT - (uint)Math.Ceiling((float)health / healthPerFrame));
        }
    }
}
