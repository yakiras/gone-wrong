using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabThrowDrop : MonoBehaviour
{
    public float maxReach = 5f;

    [SerializeField] private Transform playerCameraTrans;
    [SerializeField] private Transform propGrabPointTrans;
    [SerializeField] private LayerMask pickUpLayerMask;

    void Update()
    {
        // Grab/Drop (E)
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(playerCameraTrans.position, playerCameraTrans.forward, out RaycastHit raycastHit, maxReach, ~pickUpLayerMask))
            {
                if (raycastHit.collider.TryGetComponent(out Prop grabbedProp))
                {
                    if (!grabbedProp.isGrabbed)
                    {
                        grabbedProp.grab(propGrabPointTrans);
                    }
                    else
                    {
                        grabbedProp.drop();
                    }
                }
            }
        }

        // Throw (LMB)
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(playerCameraTrans.position, playerCameraTrans.forward, out RaycastHit raycastHit, maxReach, ~pickUpLayerMask))
            {
                if (raycastHit.collider.TryGetComponent(out Prop grabbedProp))
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
