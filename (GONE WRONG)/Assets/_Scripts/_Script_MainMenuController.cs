using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _Script_MainMenuController : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

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