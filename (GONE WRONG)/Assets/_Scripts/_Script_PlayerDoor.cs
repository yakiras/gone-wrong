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
    private Transform hinge;
    private MeshCollider doorCollider;
    private Quaternion targetRotation;
    private bool isAnimating = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isAnimating)
        {
            if (Physics.Raycast(playerCameraTrans.position, playerCameraTrans.forward, out RaycastHit raycastHit, maxReach, ~doorLayerMask))
            {
                if (raycastHit.collider.TryGetComponent(out Component door) && door.gameObject.CompareTag("Door"))
                {
                    hinge = door.transform.parent;

                    doorCollider = door.GetComponent<MeshCollider>();
                    doorCollider.enabled = false;

                    _Script_doorHinge script = hinge.GetComponent<_Script_doorHinge>();
                    targetRotation = script.isOpen ? Quaternion.Euler(0, hinge.rotation.y + script.closeChange, 0) : Quaternion.Euler(0, hinge.rotation.y + script.openChange, 0);

                    isAnimating = true;
                    hinge.GetComponent<_Script_doorHinge>().isOpen = !script.isOpen;
                }
            }
        }

        if (isAnimating)
        {
            hinge.rotation = Quaternion.Slerp(hinge.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            Debug.Log(hinge.rotation.y);

            // Stop animating if we've reached the target rotation
            if (Quaternion.Angle(hinge.rotation, targetRotation) < 0.1f)
            {
                hinge.rotation = targetRotation;
                doorCollider.enabled = true;
                isAnimating = false;
            }
        }
    }
}
