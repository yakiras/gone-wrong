using UnityEngine;
using UnityEngine.AI;

public class _Script_doorHinge : MonoBehaviour
{
    public bool version1 = true;
    public float rotateSpeed = 3f;

    private float defaultAngle;
    private bool isOpen = false;
    private bool isAnimating = false;
    private Transform hinge;
    private Quaternion targetRotation;

    private AudioSource audioSource;
    public AudioClip sfxOpen;
    public AudioClip sfxClose;
    public AudioClip sfxSlam;

    public NavMeshObstacle obstacle;

    private void Start()
    {
        obstacle = GetComponent<NavMeshObstacle>();
        audioSource = GetComponent<AudioSource>();
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

            // Stop animating if we've reached the target rotation
            if (Quaternion.Angle(hinge.rotation, targetRotation) < 0.1f)
            {
                hinge.rotation = targetRotation;
                hinge.GetChild(0).GetComponent<MeshCollider>().enabled = true;
                isAnimating = false;
                //audioSource.PlayOneShot(sfxSlam);
            }
        }

    }

    public void openClose()
    {
        if (isAnimating)
            return;

        if (isOpen)
        {
            targetRotation = Quaternion.Euler(0, defaultAngle, 0);
            audioSource.PlayOneShot(sfxClose);
            obstacle.enabled = true;
        }
        else
        {
            targetRotation = Quaternion.Euler(0, defaultAngle - 90f, 0);
            audioSource.PlayOneShot(sfxOpen);
            obstacle.enabled = false;
        }

        isAnimating = true;
        isOpen = !isOpen;
    }
}
