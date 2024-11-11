using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Script_CameraToggle : MonoBehaviour
{
    public GameObject enemiesParent;
    public float stunTime = 5;
    public _Script_PlayerCamera cameraClass;

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
                cameraClass.TurnOnCamera();
            }
            else
            {
                Debug.Log("Camera Toggle: Off");
                cameraOn = false;
                cameraClass.TurnOffCamera();
            }
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
                        Debug.Log("Enemy in sight");
                        enemyGameObj.GetComponent<_Script_SightMonsterAI>().TransformToMonster();
                        StartCoroutine(enemyGameObj.GetComponent<_Script_SightMonsterAI>().Stun(stunTime));
                    }
                }
            }
        }
    }
}
