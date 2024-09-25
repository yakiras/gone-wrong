using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    GameObject player;

    NavMeshAgent agent;

    [SerializeField] LayerMask groundLayer, playerLayer;

    Vector3 destPoint;
    // Does enemy already have a point it's walking to
    bool walkpointSet;
    [SerializeField] float range;

    bool patrolling;
    bool chasing;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        patrolling = false;
        chasing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (patrolling) Patrol();
        if (chasing) Chase();
    }

    void Chase() 
    {
        agent.SetDestination(player.transform.position);
    }

    void Patrol() 
    {
        if (!walkpointSet) 
            SearchForDest();
        if (walkpointSet)
            agent.SetDestination(destPoint);
        if (Vector3.Distance(transform.position, destPoint) < 10)
            walkpointSet = false;
    }

    void SearchForDest() 
    {
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);

        destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        // Verify that random point is does not go outside of navMesh
        if (Physics.Raycast(destPoint, Vector3.down, groundLayer))
        {
            walkpointSet=true;
        }
    }
}
