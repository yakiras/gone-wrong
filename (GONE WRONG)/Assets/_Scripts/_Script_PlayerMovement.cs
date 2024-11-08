using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Script_PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 12f;
    public float sprintSpeed = 24f;
    public float gravity = -9.81f;
    public CharacterController controller;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Vector3 lastPos;
    public bool isMoving;

    Vector3 velocity;
    bool isGrounded;
    float currentSpeed;

    public static bool movementDisabled = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (movementDisabled)
        {
            return;
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            currentSpeed = sprintSpeed;
            if (Camera.main.fieldOfView < 100f)
                Camera.main.fieldOfView += 0.2f;
        }
        else
        {
            currentSpeed = walkSpeed;
            if (Camera.main.fieldOfView > 90f)
                Camera.main.fieldOfView -= 0.2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (transform.position != lastPos)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        lastPos = transform.position;
    }

}
