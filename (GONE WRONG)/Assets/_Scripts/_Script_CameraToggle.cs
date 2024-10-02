using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Script_CameraToggle : MonoBehaviour
{
    private bool cameraOn;

    // Start is called before the first frame update
    void Start()
    {
        cameraOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!cameraOn)
            {
                Debug.Log("Camera Toggle: On");
                cameraOn = true;
                // turn on camera
                // enemy = monster
            }
            else
            {
                Debug.Log("Camera Toggle: Off");
                cameraOn = false;
                // turn off camera
                // enemy = mimic
            }
        }
    }
}
