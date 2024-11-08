using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class _Script_SightMonsterAI : MonoBehaviour
{

    // Used for transforming (set human and monster models inside the editor)
    [SerializeField] private MeshFilter monsterMesh;
    [SerializeField] private Mesh humanMesh;

    GameObject player;

    NavMeshAgent agent;

    // Used for pathfinding, set inside editor
    [SerializeField] LayerMask groundLayer, playerLayer;

    Vector3 destPoint;

    // Does enemy already have a point it's walking to
    bool walkpointSet;

    // Range that a random destination point can be set in
    [SerializeField] float range;

    bool patrolling;
    bool chasing;
    bool stunned;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        patrolling = true;
        chasing = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Used to test stun
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    StartCoroutine(Stun(5));
        //    Debug.Log("---STUNNED---");
        //}

        if (stunned)
            return;

        if (patrolling) Patrol();
        if (chasing) Chase();

        bool canSeePlayer = gameObject.GetComponent<_Script_FieldOfView>().canSeePlayer && !_Script_PlayerBed.playerHiding;
        bool playerIsMoving = player.GetComponent<_Script_PlayerMovement>().isMoving;
        if (canSeePlayer && playerIsMoving) 
        {
            chasing = true;
            patrolling = false;
        }
    }

    void Chase() 
    {
        agent.speed = 12;
        agent.SetDestination(player.transform.position);
    }

    void Patrol() 
    {
        agent.speed = 6;
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

    // Changes monster's mesh from human to monster form.
    void Transform()
    {
        monsterMesh.mesh = humanMesh;
    }


    //To use this method, do "StartCoroutine(Stun(5));" (duration is in seconds)
    IEnumerator Stun(float duration)
    {
        stunned = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(duration);
        stunned = false;
        agent.isStopped = false;
    }
}
