using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class _Script_PlayerDoor : MonoBehaviour
{
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
                if (raycastHit.collider.TryGetComponent(out Component door) && door.gameObject.CompareTag("Door"))
                {
                    doorCollider = door.GetComponent<MeshCollider>();
                    doorCollider.enabled = false;

                    door.transform.parent.GetComponent<_Script_doorHinge>().openClose();
                }
            }
        }
    }
}
