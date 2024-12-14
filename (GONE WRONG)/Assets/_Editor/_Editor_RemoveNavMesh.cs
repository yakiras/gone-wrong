using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class _Editor_RemoveNavMesh : MonoBehaviour
{
    // Start is called before the first frame update
    [MenuItem("Nav Mesh/Force Cleanup NavMesh")]
    public static void ForceCleanupNavMesh()
    {
        if (Application.isPlaying)
            return;

        NavMesh.RemoveAllNavMeshData();
    }

    [MenuItem("Nav Mesh/Remove Nav Mesh Components from scene")]
    public static void RemoveNavSurfaceComponentsFromScene()

    {

        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();



        foreach (GameObject gameObject in allGameObjects)

        {
            // Iterate through all components on the GameObject

            NavMeshSurface navmesh = gameObject.GetComponent<NavMeshSurface>();

            if (navmesh != null)

            {

                DestroyImmediate(navmesh);

            }


        }

    }
}
