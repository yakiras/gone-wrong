using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabThrowDrop : MonoBehaviour
{
    public float maxReach = 5f;
    public GameObject uiPrompts;

    [SerializeField] private Transform playerCameraTrans;
    [SerializeField] private Transform propGrabPointTrans;
    [SerializeField] private LayerMask pickUpLayerMask;
    private bool isHolding;

    private AudioSource audioSource;
    public AudioClip sfxKeyJingle;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        isHolding = false;
    }

    void Update()
    {
        // Grab/Drop (E)
        if (Physics.Raycast(playerCameraTrans.position, playerCameraTrans.forward, out RaycastHit raycastHit, maxReach, pickUpLayerMask))
        {
            if (!isHolding) uiPrompts.GetComponent<UIPrompts>().PromptE();
            else { uiPrompts.GetComponent<UIPrompts>().PromptClear(); return; }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (raycastHit.collider.TryGetComponent(out Prop grabbedProp))
                {
                    if (!grabbedProp.isGrabbed)
                    {
                        grabbedProp.grab(propGrabPointTrans);
                        isHolding = true;
                    }
                    else
                    {
                        grabbedProp.drop();
                        isHolding = false;
                    }
                }
                else if (raycastHit.collider.TryGetComponent(out Component grabbable))
                {
                    GameObject obj = grabbable.gameObject;
                    if (obj.CompareTag("Battery"))
                    {
                        _Script_CameraToggle.batteriesLeft++;
                        Destroy(obj);
                        Debug.Log("Got 1 battery!");
                    }
                    else if (obj.CompareTag("Security Key"))
                    {
                        _Script_PlayerDoor.hasSecurityKey = true;
                        Destroy(obj);
                        Debug.Log("Got security key!");
                        audioSource.PlayOneShot(sfxKeyJingle);
                    }
                    else if (obj.CompareTag("Ballroom Key"))
                    {
                        _Script_PlayerDoor.hasBallroomKey = true;
                        Destroy(obj);
                        Debug.Log("Got ballroom key!");
                        audioSource.PlayOneShot(sfxKeyJingle);
                    }
                }
            }
        }

        // Throw (LMB)
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(playerCameraTrans.position, playerCameraTrans.forward, out RaycastHit raycastHitThrow, maxReach, pickUpLayerMask))
            {
                if (raycastHitThrow.collider.TryGetComponent(out Prop grabbedProp))
                {
                    if (grabbedProp.isGrabbed)
                    {
                        grabbedProp.throwProp(playerCameraTrans);
                    }
                }
            }
        }
    }
}
