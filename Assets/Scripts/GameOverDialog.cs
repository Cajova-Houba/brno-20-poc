using Assets.Scripts.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script for dialog that acts as a game over dialog as well as pause dialog.
/// </summary>
public class GameOverDialog: MonoBehaviour
{
    /// <summary>
    /// Level this dialog is applied to. Used when restarting the level.
    /// </summary>
    public AbstractLevel level;

    public GameObject backButton;

    private bool isPaused = false;

    void Start()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Displays this dialog with back button available.
    /// </summary>
    public void DisplayAsPauseDialog()
    {
        isPaused = true;
        Time.timeScale = 0f;
        backButton.SetActive(true);
        gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(level.GetLevelSceneName());
    }

    public void DisplayMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        backButton.SetActive(false);
        gameObject.SetActive(false);
    }
    
}
