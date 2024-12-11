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
    private bool isThrown;

    public float fadeDuration = 2f; // Time it takes to fade out

    private Renderer objectRenderer;
    private Color initialColor;

    private Rigidbody propRB;
    private Transform propGrabPointTransform;

    private AudioSource audioSource;
    public AudioClip[] shatter;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        propRB = GetComponent<Rigidbody>();
        isGrabbed = false;
        isThrown = false;
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            initialColor = objectRenderer.material.color;
        }
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
        propRB.isKinematic = true;
        propRB.drag = 5f;
    }

    public void drop()
    {
        isGrabbed = false;
        propGrabPointTransform = null;
        propRB.useGravity = true;
        propRB.isKinematic = false;
        propRB.drag = 0f;
    }

    public void throwProp(Transform playerCameraTrans)
    {
        drop();
        Vector3 direction = playerCameraTrans.forward;
        propRB.AddForce(direction * throwForce);
        isThrown = true;
    }

    private void groundCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer))
        {
            propRB.isKinematic = true;
            //propRB.velocity = Vector3.zero;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer).Equals("Obstructions") && isThrown)
        {
            float soundRange = 70f;
            var sound = new Sound(transform.position, soundRange);
            Sounds.MakeSound(sound);
            audioSource.PlayOneShot(shatter[Random.Range(0, shatter.Length)]);
            StartCoroutine(FadeOut());
            isThrown = false;
        }
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Lerp alpha value over time
            float alpha = Mathf.Lerp(initialColor.a, 0f, elapsedTime / fadeDuration);
            Color newColor = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

            objectRenderer.material.color = newColor;

            yield return null; // Wait until the next frame
        }

        // Ensure alpha is set to 0
        Color finalColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        objectRenderer.material.color = finalColor;

        // Optionally destroy the GameObject after fading
        Destroy(gameObject);
    }
}
