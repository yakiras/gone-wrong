using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _Script_PlayerController : MonoBehaviour
{
    public _Script_GameOverScreen gameOverScreen;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            gameOverScreen.Show();
        }
    }
}

