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

    public AudioClip[] click;
    public AudioClip aggro;
    public AudioClip deaggro;
    public float clickInterval = 5f;
    private float stepTimer;
    private AudioSource audioSource;

    // Used for pathfinding, set inside editor
    [SerializeField] LayerMask groundLayer, playerLayer;

    // Used for patrolling
    public Transform[] waypoints;
    int waypointIndex;
    Vector3 currentDestination;
    bool goingBackToStart;
    Vector3 soundDestination;

    bool patrolling;
    bool chasing;
    bool stunned;
    bool canStun;

    public Vector3 lastPos;
    public bool isMoving;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        stepTimer = clickInterval;

        stunned = false;
        canStun = true;
        StartPatrolling();
    }

    // Update is called once per frame
    void Update()
    {
        if (stunned) return;

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

        if (chasing) 
        {
            Chase();
            if (Vector3.Distance(transform.position, soundDestination) < 1) 
            {
                Debug.Log("Going back to patrolling!!");
                StartPatrolling();
            }
        }
        if (chasing && Vector3.Distance(transform.position, soundDestination) < 1)
        {
            Debug.Log("Going back to patrolling!!");
            StartPatrolling();
        }

        if (transform.position != lastPos)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        lastPos = transform.position;

        if (!isMoving)
        {
            StartCoroutine(WaitToResumePatrolling());
        }
    }

    public void ReactToSound(Sound sound)
    {
        audioSource.PlayOneShot(aggro);
        patrolling = false;
        agent.speed = chasingSpeed;
        chasing = true;
        print($"moving towards sound at {sound.position}");
        soundDestination = sound.position;
        StartCoroutine(WaitToLoseAggro());
    }

    void Chase()
    {
        transform.LookAt(soundDestination);
        transform.position += transform.forward * chasingSpeed * Time.deltaTime;
        print(transform.position);
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

    public void StartPatrolling()
    {
        agent.speed = patrollingSpeed;
        chasing = false;
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

    public IEnumerator WaitToResumePatrolling()
    {
        for (int i = 0; i < 15; i++)
        {
            if (isMoving)
            {
                yield break;
            }
            yield return new WaitForSeconds(0.25f);
        }
        StartPatrolling();
    }

    public IEnumerator WaitToLoseAggro() 
    {
        for (int i = 0; i < 20; i++)
        {
            if (patrolling)
            {
                yield break;
            }
            yield return new WaitForSeconds(0.25f);
        }
        StartPatrolling();
    }
}
