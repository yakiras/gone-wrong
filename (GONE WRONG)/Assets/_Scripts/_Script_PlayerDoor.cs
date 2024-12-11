using UnityEngine;
using UnityEngine.SceneManagement;

public class _Script_PlayerDoor : MonoBehaviour
{
    public static bool hasSecurityKey = false;
    public static bool hasBallroomKey = false;

    public float maxReach = 5f;

    public AudioClip sfxRattle;
    public AudioClip sfxUnlock;
    private AudioSource audioSource;

    [SerializeField] private Transform playerCameraTrans;
    [SerializeField] private LayerMask doorLayerMask;
    private MeshCollider doorCollider;
    private bool isAnimating = false;
    private bool securityUnlocked = true;
    private bool ballroomUnlocked = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isAnimating)
        {
            if (Physics.Raycast(playerCameraTrans.position, playerCameraTrans.forward, out RaycastHit raycastHit, maxReach, ~doorLayerMask))
            {
                if (raycastHit.collider.TryGetComponent(out Component door))
                {
                    if (door.gameObject.CompareTag("Door"))
                    {
                        doorCollider = door.GetComponent<MeshCollider>();
                        doorCollider.enabled = false;

                        door.transform.parent.GetComponent<_Script_doorHinge>().openClose();
                    }
                    else if (door.gameObject.CompareTag("Security Door"))
                    {
                        if (!hasSecurityKey)
                        {
                            audioSource.PlayOneShot(sfxRattle);
                            return;
                        }
                        else if (!securityUnlocked)
                        {
                            audioSource.PlayOneShot(sfxUnlock);
                            securityUnlocked = true;
                            return;
                        }

                        doorCollider = door.GetComponent<MeshCollider>();
                        doorCollider.enabled = false;

                        door.transform.parent.GetComponent<_Script_doorHinge>().openClose();
                    }
                    else if (door.gameObject.CompareTag("Ballroom Door"))
                    {
                        if (!hasBallroomKey)
                        {
                            audioSource.PlayOneShot(sfxRattle);
                            return;
                        }
                        else if (!ballroomUnlocked)
                        {
                            audioSource.PlayOneShot(sfxUnlock);
                            ballroomUnlocked = true;
                            return;
                        }

                        // Attempt to load the scene
                        try
                        {
                            SceneManager.LoadScene("WinState", LoadSceneMode.Single);
                            Debug.Log("Attempting to load WinState scene");
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogError("Failed to load WinState scene: " + e.Message);
                        }
                    }
                }
            }
        }
    }
}
