using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Script_CameraToggle : MonoBehaviour
{
    public static int batteriesLeft;

    public float stunTime = 5;
    public float depleteInterval = 0.5f;
    public int depleteAmount = 5;

    public GameObject enemiesParent;
    public GameObject uiPrompts;
    public _Script_PlayerCamera cameraClass;

    public AudioClip sfxCameraOn;
    public AudioClip sfxCameraOff;

    private AudioSource audioSource;
    private bool cameraOn;
    private int batteryLevel;
    public bool canUseCamera;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        canUseCamera = true;
        cameraOn = false;
        batteryLevel = 100;
        batteriesLeft = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle camera on/off
        if (canUseCamera && Input.GetKeyDown(KeyCode.Space))
        {
            if (!cameraOn && batteryLevel > 0)
            {
                audioSource.PlayOneShot(sfxCameraOn);
                cameraOn = true;
                cameraClass.TurnOnCamera();
                StartCoroutine(DepleteBattery());
            }
            else
            {
                audioSource.PlayOneShot(sfxCameraOff);
                cameraOn = false;
                cameraClass.TurnOffCamera();
            }
        }

        // Replace battery
        if (canUseCamera && Input.GetKeyDown(KeyCode.R))
        {
            if (batteriesLeft > 0)
            {
                audioSource.PlayOneShot(sfxCameraOn);
                batteryLevel = 100;
                cameraClass.UpdateBatteryLvl(4);
                batteriesLeft--;
            }
            Debug.Log($"Remaining Batteries: {batteriesLeft}");
        }

        if (cameraOn)
        {
            Transform[] enemyTransforms = enemiesParent.GetComponentsInChildren<Transform>();
            foreach (Transform enemy in enemyTransforms)
            {
                GameObject enemyGameObj = enemy.gameObject;
                Vector3 directionToTarget = enemyGameObj.transform.position - transform.position;
                if (Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == enemyGameObj)
                    {
                        if (enemyGameObj.TryGetComponent(out _Script_SightMonsterAI sightScript))
                        {
                            //sightScript.PlayAggro();
                            StartCoroutine(sightScript.Stun(stunTime));
                        }
                        else if (enemyGameObj.TryGetComponent(out _Script_SoundMonsterAI soundScript))
                        {
                            //soundScript.PlayAggro();
                            StartCoroutine(soundScript.Stun(stunTime));
                        }
                    }
                }
            }
        }
    }

    IEnumerator DepleteBattery()
    {
        while (cameraOn && batteryLevel > 0)
        {
            yield return new WaitForSeconds(depleteInterval);

            batteryLevel -= depleteAmount;

            Debug.Log("Battery Level: " + batteryLevel + "%");

            if (batteryLevel <= 25)
            {
                StartCoroutine(uiPrompts.GetComponent<UIPrompts>().PromptR());
                cameraClass.UpdateBatteryLvl(1);
            }
            else if (batteryLevel <= 50)
                cameraClass.UpdateBatteryLvl(2);
            else if (batteryLevel <= 75)
                cameraClass.UpdateBatteryLvl(3);
            else
                cameraClass.UpdateBatteryLvl(4);

            if (batteryLevel <= 0)
            {
                audioSource.PlayOneShot(sfxCameraOff);
                cameraOn = false;
                cameraClass.TurnOffCamera();
            }
        }
    }
}
