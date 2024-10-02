using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;
    public float throwForce = 800f;
    public float lerpSpeed = 10f;

    public bool isGrabbed;

    private Rigidbody propRB;
    private Transform propGrabPointTransform;

    private void Awake()
    {
        propRB = GetComponent<Rigidbody>();
        isGrabbed = false;
    }

    private void Update()
    {
        groundCheck();
    }

    private void FixedUpdate()
    {
        if (propGrabPointTransform != null)
        {
            Vector3 newPos = Vector3.Lerp(transform.position, propGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            propRB.MovePosition(newPos);
        }
    }

    public void grab(Transform grabPointTransform)
    {
        isGrabbed = true;
        propGrabPointTransform = grabPointTransform;
        propRB.useGravity = false;
    }

    public void drop()
    {
        isGrabbed = false;
        propGrabPointTransform = null;
        propRB.useGravity = true;
        propRB.isKinematic = false;
    }

    public void throwProp(Transform playerCameraTrans)
    {
        drop();
        Vector3 direction = playerCameraTrans.forward;
        propRB.AddForce(direction * throwForce);
    }

    private void groundCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer))
        {
            propRB.isKinematic = true;
            //propRB.velocity = Vector3.zero;
        }
    }
}
