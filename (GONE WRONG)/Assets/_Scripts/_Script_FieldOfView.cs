using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    // Radius that the view can be seen
    public float radius;

    // Angle that the view can be seen
    [Range(0, 360)]
    public float angle;

    // Reference to the object being searched for, in this case the player
    public GameObject playerRef;

    // LayerMasks used for raycasting, allows use to prevent target from being seen through obstructions/walls
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");

        // Start trying to detect player. It is a coroutine so it runs simutaneously with everything else
        StartCoroutine(FOVRoutine());
    }

    // This makes it we only check if the target is in the field of view every 0.2 seconds.
    // It gets too performance-heavy if done every update. 
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        // Stores all the objects found in the radius of the transform object (enemy). Only affects
        // objects inside the targetMask layer.
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            // For now we are only searching for player so we just get the first item in the array,
            // but in the future we could do a for loop and handle all objects detected.
            Transform target = rangeChecks[0].transform;

            // Direction from where enemy is looking to where target is
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                // If the raycast is not interupted by obstruction, then the enemy can see the object
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        // If player was previously in view of enemy but not anymore than the boolean gets changed back
        else if (canSeePlayer)
            canSeePlayer = false;
    }
}