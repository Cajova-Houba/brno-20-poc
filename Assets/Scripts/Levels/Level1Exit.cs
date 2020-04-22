using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Levels
{
    /// <summary>
    /// Controler for exit from level 1.
    /// </summary>
    public class Level1Exit : MonoBehaviour
    {
        private void HandleLevelExit(PlayerControl playerControl)
        {
            SceneManager.LoadScene("Vecerka");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other.name);
            PlayerControl player = other.GetComponentInParent<PlayerControl>();

            if (player != null)
            {
                Debug.Log("PLayer exits the level!");
                HandleLevelExit(player);
            } else
            {
                Debug.Log("Not a player.");
            }
        }
    }
}
