using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Script_PlayerBed : MonoBehaviour
{
    public static bool playerHiding = false;

    public Collider bedCollider; // Collider of the bed's hiding zone

    private bool canHide = false; // Check if player in collider
    private Vector3 lastPos;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canHide)
        {
            if (!playerHiding)
                EnterHiding();
            else
                ExitHiding();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HideZone"))
            canHide = true;
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
        lastPos = transform.position;
        transform.position = new Vector3(bedCollider.transform.parent.position.x, transform.position.y - 3.2f, bedCollider.transform.parent.position.z);
        Debug.Log(transform.position);
        Debug.Log("BED");
    }

    void ExitHiding()
    {
        playerHiding = false;
        _Script_PlayerMovement.movementDisabled = false;
        transform.position = lastPos;
        Debug.Log("NO BED");
    }
}
