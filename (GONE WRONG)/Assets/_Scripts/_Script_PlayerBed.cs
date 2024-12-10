using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Script_PlayerBed : MonoBehaviour
{
    public static bool playerHiding = false;

    private Collider bedCollider; // Collider of the bed's hiding zone
    private bool canHide = false; // Check if player in collider
    private Vector3 lastPos;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canHide)
        {
            if (!playerHiding)
                EnterHiding();
            else
                StartCoroutine(ExitHiding());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HideZone"))
        {
            bedCollider = other;
            canHide = true;
            lastPos = transform.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other == bedCollider)
            canHide = false;
    }

    void EnterHiding()
    {
        playerHiding = true;
        _Script_PlayerMovement.movementDisabled = true;
        transform.position = new Vector3(bedCollider.transform.position.x, transform.position.y - 2.3f, bedCollider.transform.position.z);
        Debug.Log(transform.position);
        Debug.Log("BED");
    }

    IEnumerator ExitHiding()
    {
        transform.position = lastPos;
        yield return new WaitUntil(() => transform.position == lastPos);
        playerHiding = false;
        _Script_PlayerMovement.movementDisabled = false;
        Debug.Log("NO BED");
    }
}
