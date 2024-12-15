using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class FlipSwitch : MonoBehaviour
{
    public float rotateSpeed = 0.03f;

    private bool isAnimating;
    private Quaternion targetRotation;

    private void Start()
    {
        isAnimating = false;
        targetRotation = transform.rotation;
        targetRotation.x += 179f;
    }

    void Update()
    {
        if (isAnimating)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);

            // Stop animating if we've reached the target rotation
            //if (Quaternion.Angle(transform.rotation, targetRotation) < 0.01f)
            //{
            //    transform.rotation = targetRotation;
            //}
        }
    }

    public void Flip()
    {
        if (isAnimating)
            return;

        isAnimating = true;
    }
}
