using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabThrowDrop : MonoBehaviour
{
    public float maxReach = 5f;
    public UIPrompts uiScript;
    public JournalPrompts journalScript;

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
            if (raycastHit.collider.TryGetComponent(out Component c) && !uiScript.tutorialPlaying)
            {
                if (!isHolding && c.gameObject.layer != LayerMask.NameToLayer("Obstructions"))
                    uiScript.PromptE();
                else if (c.gameObject.layer == LayerMask.NameToLayer("Obstructions"))
                    uiScript.PromptClear();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (raycastHit.collider.TryGetComponent(out Prop grabbedProp))
                {
                    if (!grabbedProp.isGrabbed && !isHolding)
                    {
                        grabbedProp.grab(propGrabPointTrans);
                        isHolding = true;
                    }
                    else
                    {
                        grabbedProp.drop();
                        isHolding = false;
                        uiScript.PromptClear();
                    }
                }
                else if (raycastHit.collider.TryGetComponent(out Component grabbable))
                {
                    GameObject obj = grabbable.gameObject;
                    if (obj.CompareTag("Battery"))
                    {
                        _Script_CameraToggle.batteriesLeft++;
                        Destroy(obj);
                        StartCoroutine(uiScript.DisplayText($"Got: 1 battery (Total: {_Script_CameraToggle.batteriesLeft})"));
                        uiScript.PromptClear();
                    }
                    else if (obj.CompareTag("Security Key"))
                    {
                        _Script_PlayerDoor.hasSecurityKey = true;
                        Destroy(obj);
                        StartCoroutine(uiScript.DisplayText("Got: Security Key"));
                        uiScript.PromptClear();
                        journalScript.PromptJournal(2);
                        audioSource.PlayOneShot(sfxKeyJingle);
                    }
                    else if (obj.CompareTag("Ballroom Key"))
                    {
                        _Script_PlayerDoor.hasBallroomKey = true;
                        Destroy(obj);
                        StartCoroutine(uiScript.DisplayText("Got: Master Key"));
                        uiScript.PromptClear();
                        journalScript.PromptJournal(3);
                        audioSource.PlayOneShot(sfxKeyJingle);
                    }
                }
            }
        } else { if (!uiScript.tutorialPlaying) uiScript.PromptClear(); }

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
                        uiScript.PromptClear();
                        StartCoroutine(ThrowCD());
                    }
                }
            }
        }
    }

    private IEnumerator ThrowCD()
    {
        yield return new WaitForSeconds(2);
        isHolding = false;
    }
}
