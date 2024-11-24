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
    public _Script_PlayerCamera cameraClass;

    private bool cameraOn;
    private int batteryLevel;

    // Start is called before the first frame update
    void Start()
    {
        cameraOn = false;
        batteryLevel = 100;
        batteriesLeft = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle camera on/off
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!cameraOn && batteryLevel > 0)
            {
                Debug.Log("Camera Toggle: On");
                cameraOn = true;
                cameraClass.TurnOnCamera();
                StartCoroutine(DepleteBattery());
            }
            else
            {
                Debug.Log("Camera Toggle: Off");
                cameraOn = false;
                cameraClass.TurnOffCamera();
            }
        }

        // Replace battery
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (batteriesLeft > 0)
            {
                batteryLevel = 100;
                batteriesLeft--;
            }
            Debug.Log($"Remaining Batteries: {batteriesLeft}");
        }

        if (cameraOn)
        {
            // TO-DO: enemiesParent should change based on what floor the player is on
            Transform[] enemyTransforms = enemiesParent.GetComponentsInChildren<Transform>();
            foreach (Transform enemy in enemyTransforms)
            {
                GameObject enemyGameObj = enemy.gameObject;
                Vector3 directionToTarget = enemyGameObj.transform.position - transform.position;
                if (Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == enemyGameObj)
                    {
                        StartCoroutine(enemyGameObj.GetComponent<_Script_SightMonsterAI>().Stun(stunTime));
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
                cameraClass.UpdateBatteryLvl(1);
            else if (batteryLevel <= 50)
                cameraClass.UpdateBatteryLvl(2);
            else if (batteryLevel <= 75)
                cameraClass.UpdateBatteryLvl(3);
            else
                cameraClass.UpdateBatteryLvl(4);

            if (batteryLevel <= 0)
            {
                Debug.Log("Out of battery!");
                cameraOn = false;
                cameraClass.TurnOffCamera();
            }
        }
    }
}
