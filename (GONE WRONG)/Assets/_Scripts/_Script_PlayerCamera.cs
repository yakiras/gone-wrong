using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _Script_PlayerCamera : MonoBehaviour
{
    public float mouseSensitivity = 500f;
    public Transform playerBody;

    public Image cameraOverlay;
    public Sprite cameraRec;
    public Sprite cameraNoRec;

    float xRotation = 0f;
    bool cameraOn;
    bool isRec;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        TurnOffCamera();
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void TurnOffCamera()
    {
        cameraOn = false;
        cameraOverlay.enabled = false;
    }

    public void TurnOnCamera()
    {
        cameraOn = true;
        cameraOverlay.enabled = true;
        StartCoroutine(ToggleSprite());
    }

    IEnumerator ToggleSprite()
    {
        while (cameraOn)
        {
            cameraOverlay.sprite = isRec ? cameraNoRec : cameraRec;
            isRec = !isRec;

            yield return new WaitForSeconds(1f);
        }
    }
}
