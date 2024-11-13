using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class _Script_SightMonsterAI : MonoBehaviour
{
    private Animator animator;

    // Used for transforming (set human and monster models inside the editor)
    [SerializeField] private MeshFilter monsterMesh;
    [SerializeField] private Mesh humanMesh;
    [SerializeField] private Mesh sightMonsterMesh;

    GameObject player;

    NavMeshAgent agent;

    // Used for pathfinding, set inside editor
    [SerializeField] LayerMask groundLayer, playerLayer;

    // Used for patrolling
    public Transform[] waypoints;
    int waypointIndex;
    Vector3 currentDestination;
    bool goingBackToStart;

    bool patrolling;
    bool chasing;
    bool stunned;

    // Used for losing aggro only 
    bool playerOutOfSight;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        patrolling = true;
        chasing = false;
        StartPatrolling();
    }

    // Update is called once per frame
    void Update()
    {

        if (stunned)
            return;

        if (patrolling && Vector3.Distance(transform.position, currentDestination) < 1)
        {
            agent.speed = 6;
            SetNextWaypoint();
            GoToDestination();  
        }
        
        if (chasing) Chase();

        bool canSeePlayer = gameObject.GetComponent<_Script_FieldOfView>().canSeePlayer && !_Script_PlayerBed.playerHiding;
        bool playerIsMoving = player.GetComponent<_Script_PlayerMovement>().isMoving;
        if (canSeePlayer && playerIsMoving) 
        {
            Debug.Log("--AGGRO--");
            animator.SetBool("IsAggro", true);
            chasing = true;
            patrolling = false;
        }

        //If player starts to be out of sight while being chased, start the counter and lose aggro if player stays hidden for amount of time
        if (chasing && !canSeePlayer && !playerOutOfSight)
            {
                playerOutOfSight = true;
                StartCoroutine(WaitToLoseAggro());
            }
    }

    void GoToDestination()
    {
        currentDestination = waypoints[waypointIndex].position;
        agent.SetDestination(currentDestination);
    }

    void SetNextWaypoint()
    {
        if (waypointIndex == waypoints.Length - 1)
            goingBackToStart = true;
        if (waypointIndex == 0)
            goingBackToStart = false;

        if (goingBackToStart)
            waypointIndex--;
        else
            waypointIndex++;
    }

    void Chase() 
    {
        Debug.Log("CHASING");
        agent.speed = 12;
        agent.SetDestination(player.transform.position);
    }

    // Changes monster's mesh from monster to human form.
    public void TransformToHuman()
    {
        monsterMesh.mesh = humanMesh;
    }

    // Changes monster's mesh from human to monster form.
    public void TransformToMonster()
    {
        monsterMesh.mesh = sightMonsterMesh;
    }


    //To use this method, do "StartCoroutine(Stun(5));" (duration is in seconds)
    public IEnumerator Stun(float duration)
    {
        animator.SetBool("IsRevealed", true);
        stunned = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(duration);
        stunned = false;
        agent.isStopped = false;
        animator.SetBool("IsRevealed", false);
    }

    public IEnumerator WaitToLoseAggro()
    {
        for (int i = 0; i < 20; i++)
        {
            if (gameObject.GetComponent<_Script_FieldOfView>().canSeePlayer)
            {
                playerOutOfSight = false;
                yield break;
            }
            yield return new WaitForSeconds(0.25f);
        }
        if (playerOutOfSight)
        {
            animator.SetBool("IsAggro", false);
            chasing = false;
            playerOutOfSight = false;
            StartPatrolling();
        }
    }

    public void StartPatrolling()
    {
        agent.speed = 6;
        patrolling = true;
        GoToDestination();
    }
}
