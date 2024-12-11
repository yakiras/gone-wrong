using UnityEngine;
using UnityEngine.SceneManagement;

public class _Script_PlayerDoor : MonoBehaviour
{
    public static bool hasSecurityKey;
    public static bool hasBallroomKey;

    public float maxReach = 5f;
    public GameObject uiPrompts;
    private UIPrompts uiScript;

    public AudioClip sfxRattle;
    public AudioClip sfxUnlock;
    private AudioSource audioSource;

    [SerializeField] private Transform playerCameraTrans;
    [SerializeField] private LayerMask doorLayerMask;
    private MeshCollider doorCollider;
    private bool isAnimating = false;
    private bool securityUnlocked;
    private bool ballroomUnlocked;

    void Start()
    {
        uiScript = uiPrompts.GetComponent<UIPrompts>();
        hasSecurityKey = false;
        securityUnlocked = false;
        hasBallroomKey = false;
        ballroomUnlocked = false;
    }

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
                        audioSource = door.transform.parent.GetComponent<AudioSource>();
                        doorCollider = door.GetComponent<MeshCollider>();
                        doorCollider.enabled = false;

                        door.transform.parent.GetComponent<_Script_doorHinge>().openClose();
                    }
                    else if (door.gameObject.CompareTag("Security Door"))
                    {
                        audioSource = door.transform.parent.GetComponent<AudioSource>();
                        if (!hasSecurityKey)
                        {
                            StartCoroutine(uiScript.DisplayText("Needs: Security Key"));
                            audioSource.PlayOneShot(sfxRattle);
                            return;
                        }
                        else if (!securityUnlocked)
                        {
                            StartCoroutine(uiScript.DisplayText("Used: Security Key"));
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
                        audioSource = door.transform.parent.GetComponent<AudioSource>();
                        if (!hasBallroomKey)
                        {
                            StartCoroutine(uiScript.DisplayText("Needs: Master Key"));
                            audioSource.PlayOneShot(sfxRattle);
                            return;
                        }
                        else if (!ballroomUnlocked)
                        {
                            StartCoroutine(uiScript.DisplayText("Used: Master Key"));
                            audioSource.PlayOneShot(sfxUnlock);
                            ballroomUnlocked = true;
                            return;
                        }

                        // Attempt to load the scene
                        try
                        {
                            Cursor.lockState = CursorLockMode.None;
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
