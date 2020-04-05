using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class SettingsMenu : MonoBehaviour
    {
        public Slider enemyRateSlider;

        public Slider tramRateSlider;
        
        public void SetEnemySpawnRate(float sliderValue)
        {
            SettingsHolder.enemySpawnRate = sliderValue;
        }

        public void SetTramSpawnRate(float sliderValue)
        {
            SettingsHolder.tramSpawnRate = sliderValue;
        }

        void Start()
        {
            // make sure the sliders 'rememder' the value they were set to
            enemyRateSlider.value = SettingsHolder.enemySpawnRate;
            tramRateSlider.value = SettingsHolder.tramSpawnRate;
        }
    }
}
