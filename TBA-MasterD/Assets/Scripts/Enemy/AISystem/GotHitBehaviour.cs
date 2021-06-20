using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotHitBehaviour : AIBehaviour
{
    //Flag Indicating if this Behaviour is Active
    private bool isActive;

    //Player Reference
    private Transform target;

    //Enemy's Position when Got Hit
    private Vector3 currentSelfPosition;

    //Target's Position when Self Got Hit
    private Vector3 currentTargetPosition;

    //Enemy's EulerAngle's Rotation when Got Hit
    private Vector3 currentSelfRotation;

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


    public GotHitBehaviour(MonoBehaviour self, AIStateMachine stateMachine) : base(self, stateMachine, "GotHit")
    {
        
    }

    public override void Init()
    {
        //Get Player Reference
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void OnBehaviourEnd()
    {
        Debug.Log("Got Hit Behaviour Ended");
        
        //Set IsActive to False, and Reset all the other Flags.
        isActive = false;
        hasStartedToRotate = false;
        hasRotated = false;
    }

    public override void OnBehaviourStart()
    {
        Debug.Log("Got Hit Behaviour Started");

        //Set IsActive to True, Set all Flags to False.
        isActive = true;
        hasStartedToRotate = false;
        hasRotated = false;
    }

    public override void OnUpdate()
    {
        //If IsActive is False, return.
        if (!isActive)
        {
            return;
        }

        RotateTowardsTarget();
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
            currentSelfPosition = self.transform.localPosition;
            currentTargetPosition = target.transform.localPosition;

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
            //stateMachine.HandleEvent(AIEvents.PlayerNotFound);
        }
    }
}
