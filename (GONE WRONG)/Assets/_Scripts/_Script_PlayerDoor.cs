using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _Script_PlayerDoor : MonoBehaviour
{
    public static bool hasSecurityKey = false;
    public static bool hasBallroomKey = false;

    public float maxReach = 5f;

    [SerializeField] private Transform playerCameraTrans;
    [SerializeField] private LayerMask doorLayerMask;
    private MeshCollider doorCollider;
    private bool isAnimating = false;

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
                            return;

                        doorCollider = door.GetComponent<MeshCollider>();
                        doorCollider.enabled = false;

                        door.transform.parent.GetComponent<_Script_doorHinge>().openClose();
                    }
                    else if (door.gameObject.CompareTag("Ballroom Door"))
                    {
                        if (!hasBallroomKey)
                            return;

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
