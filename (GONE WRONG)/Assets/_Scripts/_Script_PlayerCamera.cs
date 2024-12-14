using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _Script_PlayerCamera : MonoBehaviour
{
    public float mouseSensitivity = 300f;
    public Transform playerBody;

    public Image cameraOverlay;
    public Sprite cameraRec;
    public Sprite cameraNoRec;

    public Image batteryOverlay;
    public Sprite battery4;
    public Sprite battery3;
    public Sprite battery2;
    public Sprite battery1;

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
        batteryOverlay.enabled = false;
    }

    public void TurnOnCamera()
    {
        cameraOn = true;
        cameraOverlay.enabled = true;
        batteryOverlay.enabled = true;
        StartCoroutine(ToggleSprite());
    }

    public void UpdateBatteryLvl(int batteryLevel)
    {
        switch (batteryLevel)
        {
            case 1:
                batteryOverlay.sprite = battery1;
                break;
            case 2:
                batteryOverlay.sprite = battery2;
                break;
            case 3:
                batteryOverlay.sprite = battery3;
                break;
            case 4:
                batteryOverlay.sprite = battery4;
                break;
        }
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
