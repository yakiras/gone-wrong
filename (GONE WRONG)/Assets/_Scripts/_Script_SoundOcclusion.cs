using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundOcclusion : MonoBehaviour
{
    public Transform listener;          // Reference to the player or AudioListener
    public LayerMask obstructionMask;   // Layer for walls or objects that block sound
    public float maxVolume = 1.0f;      // Full volume
    public float occludedVolume = 0.2f; // Reduced volume when blocked

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (listener == null) return;

        // Perform raycast
        Vector3 direction = listener.position - transform.position;
        float distance = direction.magnitude;
        Ray ray = new Ray(transform.position, direction.normalized);

        if (Physics.Raycast(ray, out RaycastHit hit, distance, obstructionMask))
        {
            // Sound is blocked
            audioSource.volume = occludedVolume;
        }
        else
        {
            // Sound is not blocked
            audioSource.volume = maxVolume;
        }
    }
}
