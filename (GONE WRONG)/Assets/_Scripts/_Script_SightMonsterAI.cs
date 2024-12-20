using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public int patrollingSpeed;
    public int chasingSpeed;
    public int stunCoolDown;

    public GameObject player;

    public AudioClip[] click;
    public AudioClip aggro;
    public AudioClip deaggro;
    public float clickInterval = 5f;
    private float stepTimer;
    private AudioSource audioSource;

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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        stepTimer = clickInterval;

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

        if (patrolling)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayClick();
                stepTimer = clickInterval;
            }
        }

        if (patrolling && Vector3.Distance(transform.position, currentDestination) < 1)
        {
            SetNextWaypoint();
            GoToDestination();
        }

        if (chasing) Chase();

        bool canSeePlayer = gameObject.GetComponent<_Script_FieldOfView>().canSeePlayer && !_Script_PlayerBed.playerHiding;
        bool playerIsMoving = player.GetComponent<_Script_PlayerMovement>().isMoving;
        if (canSeePlayer && playerIsMoving && !chasing) 
        {
            audioSource.PlayOneShot(aggro);
            animator.SetBool("IsAggro", true);
            chasing = true;
            patrolling = false;
            agent.isStopped = true;
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
        //agent.isStopped = false;
        //agent.speed = chasingSpeed;
        //agent.SetDestination(player.transform.position);
        //Debug.Log("Destination:" + agent.destination);
        //Debug.Log("Player location:" + player.transform.position);
        transform.LookAt(player.transform);
        transform.position += transform.forward * chasingSpeed * Time.deltaTime;
    }

    //To use this method, do "StartCoroutine(Stun(5));" (duration is in seconds)
    public IEnumerator Stun(float duration)
    {
        if (!canStun)
            yield break;

        audioSource.PlayOneShot(deaggro);
        animator.SetBool("IsRevealed", true);
        stunned = true;
        canStun = false;
        agent.isStopped = true;
        yield return new WaitForSeconds(duration);
        stunned = false;
        agent.isStopped = false;
        animator.SetBool("IsRevealed", false);

        StartCoroutine(StunCD());
    }

    public IEnumerator StunCD()
    {
        yield return new WaitForSeconds(stunCoolDown);
        canStun = true;
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
            audioSource.PlayOneShot(deaggro);
        }
    }

    public void StartPatrolling()
    {
        agent.speed = patrollingSpeed;
        patrolling = true;
        GoToDestination();
    }

    public void PlayAggro()
    {
        audioSource.PlayOneShot(aggro);
    }

    private void PlayClick()
    {
        if (click.Length > 0)
        {
            // Choose a random click clip to play
            AudioClip clip = click[Random.Range(0, click.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
}
