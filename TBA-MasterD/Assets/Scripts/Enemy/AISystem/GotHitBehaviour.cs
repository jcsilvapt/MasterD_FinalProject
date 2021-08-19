using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class GotHitBehaviour : AIBehaviour
{
    //animator
    private Animator anim;

    //Flag Indicating if this Behaviour is Active
    private bool isActive;

    //Player Reference
    private Transform target;

    //Nav Mesh Agent Reference;
    private NavMeshAgent agent;

    //Enemy's Position when Got Hit
    private Vector3 currentSelfPosition;

    //Target's Position when Self Got Hit
    private Vector3 currentTargetPosition;

    //Enemy's EulerAngle's Rotation when Got Hit
    private Vector3 currentSelfRotation;

    //Distance between Enemy and Target
    private float distance;

    //Flag Indicating if the Distance between the Enemy and the Player is already calculated
    private bool isDistanceCalculated;

    //Flag Indicating if the Enemy Started to Rotate Towards Target
    private bool hasStartedToRotate;

    //Flag Indicating if the Enemy Has Rotated Towards Target
    private bool hasRotated;

    //Rotation Angle Necessary for Enemy to be Facing Target
    private float angleBetweenEnemyAndTarget;

    //Time it Takes to Rotate Towards Target
    private float timeToRotate;

    //Time the Rotation Started
    private float timeTheRotationStarted;

    //Flag Indicating if Enemy's Destination is Set
    private bool isDestinationSet;

    //Flag Indicating if the Enemy has approached the place where the Shot took Place
    private bool isInShotPlace;

    //Flag Indicating if the Enemy has approached the place where it will take Cover.
    private bool isInCoverPlace;

    /* Objectives:
     *  - Short Distance Hit:
     *    The Enemy will rotate towards the Player and Shoot.
     *  
     *  - Medium Distance Hit:
     *    The Enemy will look for the Player where he shot.
     *    
     *  - Long Distance Hit:
     *    The enemy will look for cover against the direction where he was shot.
     */


    public GotHitBehaviour(MonoBehaviour self, AIStateMachine stateMachine) : base(self, stateMachine, "GotHit")
    {
        anim = self.GetComponent<Animator>();
    }

    public override void Init()
    {
        //Get Player Reference
        target = GameObject.FindGameObjectWithTag("Player").transform;

        //Get Nav Mesh Agent Component
        agent = self.gameObject.GetComponent<NavMeshAgent>();
    }

    public override void OnBehaviourEnd()
    {
        Debug.Log("Got Hit Behaviour Ended");
        
        //Set IsActive to False, and Reset all the other Flags.
        //Set Distance to 0
        isActive = false;
        isDistanceCalculated = false;
        hasStartedToRotate = false;
        hasRotated = false;
        isDestinationSet = false;
        isInShotPlace = false;
        isInCoverPlace = false;
        anim.SetBool("iChase", false);
        
        distance = 0;
    }

    public override void OnBehaviourStart()
    {
        Debug.Log("Got Hit Behaviour Started");

        //Set IsActive to True, Set all Flags to False.
        //Set Distance to 0
        isActive = true;
        isDistanceCalculated = false;
        hasStartedToRotate = false;
        hasRotated = false;
        isDestinationSet = false;
        isInShotPlace = false;
        isInCoverPlace = false;
        anim.SetBool("iChase", true);
        distance = 0;
    }

    public override void OnUpdate()
    {
        //If IsActive is False, return.
        if (!isActive)
        {
            return;
        }

        GetDistance();

        ReactToHitDistanceShort();
        ReactToHitDistanceMedium();
        ReactToHitDistanceLong();
    }

    private void GetDistance()
    {
        //If the Distance is already Calculated, return.
        if (isDistanceCalculated)
        {
            return;
        }

        //Get The Local Positions of both the Enemy and the Player when the Hit Ocurred
        currentSelfPosition = self.transform.localPosition;
        currentTargetPosition = target.transform.localPosition;

        //Get the Direction between them and, consequently, the Distance.
        Vector3 direction = currentTargetPosition - currentSelfPosition;
        distance = direction.magnitude;

        //Set Is Distance Calculated to True.
        isDistanceCalculated = true;
        
    }

    private void ReactToHitDistanceShort()
    {
        //If the Distance is Greater than the Short Distance, return.
        if (distance > 10)
        {
            return;
        }

        //Rotate Towards Player
        RotateTowardsTarget();
    }

    private void ReactToHitDistanceMedium()
    {
        //If the Distance is Lesser than the Short Distance and Greater than the Medium Distance, return.
        if (distance <= 10 || distance > 20)
        {
            return;
        }
        //If the Player already reached the Destination, return.
        if (isInShotPlace)
        {
            return;
        }

        //If the Enemy Destination isn't Set, set it.
        if (!isDestinationSet)
        {
            agent.SetDestination(currentTargetPosition);
            
            isDestinationSet = true;
       
        }

        //If Enemy has Vision of Player, Call HandleEvent SeePlayer
        if (AIUtils_Fabio.HasVisionOfPlayer(self.transform, target, self.GetComponent<Enemy>().GetDistanceToView()))
        {
            stateMachine.HandleEvent(AIEvents.SeePlayer);
            return;
        }

        //If Enemy Reached Shot Position, Set flag to true.
        //If Enemy does not have Vision of Player, Call HandleEvent PlayerNotFound
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            isInShotPlace = true;

            stateMachine.HandleEvent(AIEvents.LostPlayer);
        }
        
    }

    private void ReactToHitDistanceLong()
    {
        //If the Distance is Lesser than the Medium Distance, return.
        if (distance <= 20)
        {
            return;
        }

        //If the Player already reached the Destination, return.
        if (isInCoverPlace)
        {
            return;
        }

        //If the Enemy Destination isn't Set, set it.
        if (!isDestinationSet)
        {
            List<NavMeshHit> hitList = new List<NavMeshHit>();
            NavMeshHit navHit;

            // Create Random Points around the player so we can find the Nearest Point
            for (int i = 0; i < 15; i++)
            {
                //Spawn Point of Random Hits
                Vector3 spawnPoint = self.transform.position;

                //Offset to the Random Hits, created with a unit circle in which the radius will be the iteration number
                Vector2 offset = Random.insideUnitCircle * i;

                //Adding them, updating it each iteration
                spawnPoint.x += offset.x;
                spawnPoint.z += offset.y;

                //Finding Closest Edge from the Enemy
                NavMesh.FindClosestEdge(spawnPoint, out navHit, NavMesh.AllAreas);

                //Add that to the list.
                hitList.Add(navHit);
            }

            // Sort the list by distance using Linq
            var sortedList = hitList.OrderBy(x => x.distance);

            // Loop through the Sorted List and Check if the hit Normal doesn't point towards the enemy.
            // If it doesn't point towards the enemy, navigate the agent to that position and break the loop as this is the closest cover for the agent. (Because the list is sorted on distance)
            foreach (NavMeshHit hit in sortedList)
            {
                if (Vector3.Dot(hit.normal, (target.transform.position - self.transform.position)) < Random.Range(-0.5f, 0f))
                {
                    agent.SetDestination(hit.position);

                    isDestinationSet = true;

                    break;
                }
            }
        }

        //If Enemy has Vision of Player, Call HandleEvent SeePlayer
        if (AIUtils_Fabio.HasVisionOfPlayer(self.transform, target, self.GetComponent<Enemy>().GetDistanceToView()))
        {
            stateMachine.HandleEvent(AIEvents.SeePlayer);
            return;
        }

        //If Enemy Reached Shot Position, Set flag to true.
        //If Enemy does not have Vision of Player, Call HandleEvent PlayerNotFound
        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            isInCoverPlace = true;

                     
            stateMachine.HandleEvent(AIEvents.ReachedDestination);
            return;
        }
    }

    private void RotateTowardsTarget()
    {
        //If the Enemy has Already Rotated towards Target, return.
        if (hasRotated)
        {
            return;
        }

        //If the Enemy hasn't Started to Rotate yet, Take his current position and current Rotation,
        //Get the Direction between the Enemy and the Target's current position and, from this, 
        //the Angle between Enemy's position and Target's Position,
        //If the Angle is negative, add 360 degrees to it.
        //Set the Time it started to Rotate (now), and set the flag Has Started To Rotate to true.
        if (!hasStartedToRotate)
        {
            currentSelfRotation = self.transform.localEulerAngles;

            Vector3 direction = currentTargetPosition - currentSelfPosition;

            angleBetweenEnemyAndTarget = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            if(angleBetweenEnemyAndTarget < 0)
            {
                angleBetweenEnemyAndTarget += 360f;
            }

            timeTheRotationStarted = Time.time;
            timeToRotate = (0.6f / 180) * angleBetweenEnemyAndTarget + 0.1f;

            hasStartedToRotate = true;
        }

        //Rotation Complete will take the Value of 1 when the Rotation is Done.
        float rotationCompletion = (Time.time - timeTheRotationStarted) / timeToRotate;

        //Rotates the Enemy towards the Target
        self.transform.localEulerAngles = new Vector3(0, Mathf.Lerp(currentSelfRotation.y, angleBetweenEnemyAndTarget, rotationCompletion), 0);

        //If in the process, the Enemy sees the Player, Call the HandleEvent and return.
        if (AIUtils_Fabio.HasVisionOfPlayer(self.transform, target, self.GetComponent<Enemy>().GetDistanceToView()))
        {
            stateMachine.HandleEvent(AIEvents.SeePlayer);
            return;
        }

        //Rotation Completion Reached 1, the rotation is done, set the flag to True
        if (rotationCompletion >= 1)
        {
            hasRotated = true;

            //The Enemy doesn't see the Player, call the HandleEvent and end this Behaviour.
            stateMachine.HandleEvent(AIEvents.LostPlayer);
        }
    }
}
