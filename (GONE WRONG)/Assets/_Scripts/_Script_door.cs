using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Script_doorHinge : MonoBehaviour
{
    public bool version1 = true;
    public float rotateSpeed = 3f;

    private float defaultAngle;
    private bool isOpen = false;
    private Transform hinge;
    private bool isAnimating = false;
    private Quaternion targetRotation;

    private void Start()
    {
        hinge = gameObject.transform;
        if (version1)
            defaultAngle = 0f;
        else
            defaultAngle = 180f;
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

        targetRotation = isOpen ? Quaternion.Euler(0, defaultAngle, 0) : Quaternion.Euler(0, defaultAngle - 90f, 0);

        isAnimating = true;
        isOpen = !isOpen;
    }
}
