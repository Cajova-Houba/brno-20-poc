using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Generic
{
    /// <summary>
    /// Abstract class for levels. Time is freezed in the Start() method and then started in OnIntroEnd().
    /// </summary>
    public abstract class AbstractLevel : MonoBehaviour
    {
        public PlayerControl player;

        /// <summary>
        /// Returns the name of the scene for this level.
        /// </summary>
        /// <returns></returns>
        public abstract string GetLevelSceneName();

        /// <summary>
        /// Returns the number of enemies killed by player in this level so far.
        /// </summary>
        /// <returns></returns>
        public abstract int GetEnemiesKilled();

        public GameOverDialog pauseDialog;

        /// <summary>
        /// How long the player dying animation lasts.
        /// </summary>
        public float dyingAnimationDuration;

        /// <summary>
        /// Audio source for the level music.
        /// </summary>
        public AudioSource levelMusic;

        /// <summary>
        /// May be called by IntroPlayer.
        /// </summary>
        public virtual void OnIntroEnd()
        {
            Time.timeScale = 1f;
            if (levelMusic != null)
            {
                levelMusic.Play();
            }
        }

        protected bool playerDied;

        protected bool CheckDeadPlayer()
        {
            return GameObject.FindGameObjectWithTag(SettingsHolder.deadPlayerTagName) != null;
        }

        protected virtual void Start()
        {
            playerDied = false;
            
            // turned on by intro
            Time.timeScale = 0f;
        }

        /// <summary>
        /// Level-specific update.
        /// </summary>
        protected abstract void HandleLevelUpdate();

        void Update()
        {
            bool deadPlayer = false;
            if (!playerDied)
            {
                deadPlayer = CheckDeadPlayer();
            }

            if (!deadPlayer)
            {
                HandleLevelUpdate();
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    pauseDialog.DisplayAsPauseDialog();
                }
            }
            else if (!playerDied)
            {
                Debug.Log("Player died.");

                // deadPlayer flag indicates that deadPlayer object was found in this Update() method
                // but since playerDied flag is not set, it means that it's the first time this object
                // was found -> wait a bit and display game over dialog
                playerDied = true;
                StartCoroutine(RestartDialogCoroutine());
            }
        }

        private IEnumerator RestartDialogCoroutine()
        {
            yield return new WaitForSeconds(dyingAnimationDuration);
            Debug.Log("Displaying game over dialog.");
            pauseDialog.gameObject.SetActive(true);
        }
    }
}
