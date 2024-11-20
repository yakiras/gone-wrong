using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Script_doorHinge : MonoBehaviour
{
    public bool isOpen = false;
    public float rotateSpeed = 3f;
    public float openAngle = 90f;

    private Transform hinge;
    private bool isAnimating = false;
    private Quaternion targetRotation;

    private void Start()
    {
        hinge = gameObject.transform;
    }

    void Update()
    {
        if (isAnimating)
        {
            hinge.rotation = Quaternion.Lerp(hinge.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            Debug.Log(hinge.rotation.y);

            // Stop animating if we've reached the target rotation
            if (Quaternion.Angle(hinge.rotation, targetRotation) < 0.1f)
            {
                hinge.rotation = targetRotation;
                hinge.GetChild(0).GetComponent<MeshCollider>().enabled = true;
                isAnimating = false;
            }
        }
    }

    public void openClose()
    {
        if (isAnimating)
            return;

        Debug.Log(hinge.parent.rotation.y);

        targetRotation = isOpen ? Quaternion.Euler(0, hinge.parent.rotation.y, 0) : Quaternion.Euler(0, hinge.parent.rotation.y - openAngle, 0);

        isAnimating = true;
        isOpen = !isOpen;
    }
}
