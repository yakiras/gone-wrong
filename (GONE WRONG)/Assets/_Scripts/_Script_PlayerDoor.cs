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
    private Quaternion closeRotation;
    private Quaternion openRotation;
    private bool isAnimating = false;
    private bool isOpen = false;

    private void Start()
    {
        closeRotation = Quaternion.identity;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closeRotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isAnimating)
        {
            if (Physics.Raycast(playerCameraTrans.position, playerCameraTrans.forward, out RaycastHit raycastHit, maxReach, ~doorLayerMask))
            {
                if (raycastHit.collider.TryGetComponent(out Component door))
                {
                    doorHinge = door.transform.parent;
                    doorCollider = door.GetComponent<MeshCollider>();
                    doorCollider.enabled = false;
                    isAnimating = true;
                    isOpen = !isOpen;
                }
            }
        }

        if (isAnimating)
        {
            Quaternion targetRotation = isOpen ? closeRotation : openRotation;
            doorHinge.rotation = Quaternion.Slerp(doorHinge.rotation, targetRotation, Time.deltaTime * rotateSpeed);

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
