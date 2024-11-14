using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _Script_Key : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected with: " + other.gameObject.name); // Check that trigger works

        // Check if the object that triggered the collider has the tag "WinState"
        if (other.CompareTag("WinState"))
        {
            Debug.Log("Entered trigger with WinState object"); // Confirmed tag check

            // Attempt to load the scene
            try
            {
                SceneManager.LoadScene("WinState", LoadSceneMode.Single);
                Debug.Log("Attempting to load WinState scene");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to load WinState scene: " + e.Message);
            }
        }
        else
        {
            Debug.Log("Trigger entered with object, but it does not have the 'WinState' tag");
        }
    }
}
