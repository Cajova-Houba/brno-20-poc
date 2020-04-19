using Assets.Scripts.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script for dialog that acts as a game over dialog as well as pause dialog.
/// </summary>
public class GameOverDialog : MonoBehaviour
{
    /// <summary>
    /// Level this dialog is applied to. Used when restarting the level.
    /// </summary>
    public AbstractLevel level;

    void Start()
    {
        gameObject.SetActive(false);
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
}
