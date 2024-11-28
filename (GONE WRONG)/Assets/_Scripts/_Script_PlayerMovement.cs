using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Script_PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 12f;
    public float sprintSpeed = 24f;
    public float defaultFOV = 70f;
    public float FOVchange = 10f;
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

    // Footstep SFX
    private AudioSource audioSource;
    public AudioClip[] walkClips;
    public AudioClip[] runClips;
    public float walkInterval = 0.5f;
    public float runInterval = 0.25f;
    private float stepTimer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Camera.main.fieldOfView = defaultFOV;
    }

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
            if (Camera.main.fieldOfView < (defaultFOV + FOVchange))
                Camera.main.fieldOfView += 0.2f;

            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayRunSound();
                stepTimer = runInterval;
            }
        }
        else
        {
            currentSpeed = walkSpeed;
            if (Camera.main.fieldOfView > defaultFOV)
                Camera.main.fieldOfView -= 0.2f;
            if (Input.GetKey(KeyCode.W))
            {
                stepTimer -= Time.deltaTime;
                if (stepTimer <= 0f)
                {
                    var sound = new Sound(transform.position, 30f);
                    Sounds.MakeSound(sound);
                    PlayWalkSound();
                    stepTimer = walkInterval;
                }
            }
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

    void PlayWalkSound()
    {
        if (walkClips.Length > 0)
        {
            // Choose a random walk clip to play
            AudioClip clip = walkClips[Random.Range(0, walkClips.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
    
    void PlayRunSound()
    {
        if (runClips.Length > 0)
        {
            // Choose a random run clip to play
            AudioClip clip = runClips[Random.Range(0, runClips.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
}
