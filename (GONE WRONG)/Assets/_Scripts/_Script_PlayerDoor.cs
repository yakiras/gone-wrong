using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class _Script_PlayerDoor : MonoBehaviour
{
    public float maxReach = 5f;
    public float rotateSpeed = 3f;
    public float openAngle = 90f;

    [SerializeField] private Transform playerCameraTrans;
    [SerializeField] private LayerMask doorLayerMask;
    private Transform doorHinge;
    private MeshCollider doorCollider;
    private Quaternion targetRotation;
    private bool isAnimating = false;
    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isAnimating)
        {
            if (Physics.Raycast(playerCameraTrans.position, playerCameraTrans.forward, out RaycastHit raycastHit, maxReach, ~doorLayerMask))
            {
                if (raycastHit.collider.TryGetComponent(out Component door) && door.gameObject.CompareTag("Door"))
                {
                    doorHinge = door.transform.parent;
                    doorCollider = door.GetComponent<MeshCollider>();
                    doorCollider.enabled = false;

                    targetRotation = isOpen ? Quaternion.Euler(0, doorHinge.rotation.y, 0) : Quaternion.Euler(0, doorHinge.rotation.y, 0);

                    isAnimating = true;
                    isOpen = !isOpen;
                }
            }
        }

        if (isAnimating)
        {
            doorHinge.rotation = Quaternion.Slerp(doorHinge.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            Debug.Log(doorHinge.rotation.y);

            // Stop animating if we've reached the target rotation
            if (Quaternion.Angle(doorHinge.rotation, targetRotation) < 0.1f)
            {
                doorHinge.rotation = targetRotation;
                doorCollider.enabled = true;
                isAnimating = false;
            }
        }
    }
}
