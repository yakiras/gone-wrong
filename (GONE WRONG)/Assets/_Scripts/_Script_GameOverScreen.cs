using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class _Script_GameOverScreen : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        gameObject.SetActive(false);
    }

    public void Show() 
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Retry()
    {
        SceneManager.UnloadSceneAsync("Floorplan Testing");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        SceneManager.UnloadSceneAsync("Floorplan Testing");
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
}
