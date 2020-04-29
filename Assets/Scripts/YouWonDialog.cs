using Assets.Scripts.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class YouWonDialog : MonoBehaviour
    {

        void Start()
        {
            gameObject.SetActive(true);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene("Level1");
        }

        public void DisplayMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
