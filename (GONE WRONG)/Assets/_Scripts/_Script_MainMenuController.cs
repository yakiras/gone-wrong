using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _Script_MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Floorplan Testing");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void TitleScreen()
    {
        SceneManager.LoadScene("Main Menu");
    }
}