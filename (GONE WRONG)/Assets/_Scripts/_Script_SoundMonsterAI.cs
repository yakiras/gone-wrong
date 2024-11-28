using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class _Script_SoundMonsterAI : MonoBehaviour
{
    private Animator animator;

    public int patrollingSpeed;
    public int chasingSpeed;
    public int stunCoolDown;

    public GameObject player;

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
    bool canStun;

    // Used for losing aggro only 
    bool playerOutOfSight;

    public void ReactToSound(Sound sound) 
    {
        print("reacting to sound");
    }


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        patrolling = true;
        chasing = false;
        stunned = false;
        canStun = true;
        StartPatrolling();
    }

    // Update is called once per frame
    void Update()
    {

        if (stunned)
            return;

        if (patrolling && Vector3.Distance(transform.position, currentDestination) < 1)
        {
            SetNextWaypoint();
            GoToDestination();
        }
    }

    void GoToDestination()
    {
        currentDestination = waypoints[waypointIndex].position;
        agent.SetDestination(currentDestination);
        Debug.Log(agent.destination);
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

    //To use this method, do "StartCoroutine(Stun(5));" (duration is in seconds)
    public IEnumerator Stun(float duration)
    {
        if (!canStun)
            yield break;

        animator.SetBool("IsRevealed", true);
        stunned = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(duration);
        stunned = false;
        agent.isStopped = false;
        animator.SetBool("IsRevealed", false);

        StartCoroutine(StunCD());
    }

    public IEnumerator StunCD()
    {
        canStun = false;
        yield return new WaitForSeconds(stunCoolDown);
        canStun = true;
    }

    public void StartPatrolling()
    {
        agent.speed = patrollingSpeed;
        patrolling = true;
        GoToDestination();
    }
}
