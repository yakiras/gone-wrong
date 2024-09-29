using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _Script_PlayerController : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Collision Detected");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

