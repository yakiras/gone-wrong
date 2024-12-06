using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _Script_PlayerController : MonoBehaviour
{
    public Vector3 spawnPoint;
    public _Script_GameOverScreen gameOverScreen;

    private void Start()
    {
        float x = PlayerPrefs.GetFloat("checkpointX", 0);
        float y = PlayerPrefs.GetFloat("checkpointY", 0);
        float z = PlayerPrefs.GetFloat("checkpointZ", 0);

        if (x == 0 && y == 0 && z == 0) return;
        Vector3 startingPosition = new Vector3(x, y, z);
        transform.position = startingPosition;
        Debug.Log("setting start position to " + startingPosition);
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            gameOverScreen.Show();
        }

        else if (collision.gameObject.tag == "Checkpoint")
        {
            Vector3 playerPosition = GameObject.Find("Player").transform.position;
            PlayerPrefs.SetFloat("checkpointX", playerPosition.x);
            PlayerPrefs.SetFloat("checkpointY", playerPosition.y);
            PlayerPrefs.SetFloat("checkpointZ", playerPosition.z);
            print("Checkpoint set");
        }
    }
}

