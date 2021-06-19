using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotHitBehaviour : AIBehaviour
{
    //Flag Indicating if this Behaviour is Active
    private bool isActive;

    //Player Reference
    private Transform target;

    //Initial Enemy Rotation
    private Vector3 enemyRotation;

    //Target's Position when Self Got Hit
    private Vector3 currentTargetPosition;

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

        //If the Enemy hasn't Started to Rotate yet, Take his current Rotation,
        //Get the Direction between the Enemy and the Target's current position and, from this, 
        //the Angle between Enemy's forward and Target's Position,
        //Set the Time it started to Rotate (now), and set the flag Has Started To Rotate to true.
        if (!hasStartedToRotate)
        {
            enemyRotation = self.transform.eulerAngles;

            currentTargetPosition = target.position;
            Vector3 direction = currentTargetPosition - self.transform.position;

            //if(angle)
            
            angleBetweenEnemyAndTarget = Vector3.Angle(self.transform.forward, direction);


            timeTheRotationStarted = Time.time;
            timeToRotate = (0.6f / 180) * angleBetweenEnemyAndTarget + 0.1f;

            hasStartedToRotate = true;

            Debug.Log("Enemy EulerAngles Y: " + self.transform.eulerAngles.y + 
                      " | Player Position: " + Mathf.Atan2(target.transform.position.x, target.transform.position.z) * Mathf.Rad2Deg + 
                      " | Angle Between Both: " + angleBetweenEnemyAndTarget + 
                      " | Direction: " + Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg);

            if(Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg < 0)
            {
                self.transform.eulerAngles = new Vector3(self.transform.eulerAngles.x, self.transform.eulerAngles.y - 360, self.transform.eulerAngles.z);
                Debug.Log("Entered " + Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + " | " + self.transform);
            }
        }

        //Rotation Complete will take the Value of 1 when the Rotation is Done.
        float rotationCompletion = (Time.time - timeTheRotationStarted) / timeToRotate;

        //Rotates the Enemy towards the Target
        self.transform.eulerAngles = new Vector3(0, Mathf.Lerp(enemyRotation.y, Mathf.Atan2(currentTargetPosition.x, currentTargetPosition.z) * Mathf.Rad2Deg, rotationCompletion), 0);

        //Rotation Completion Reached 1, the rotation is done, set the flag to True
        if (rotationCompletion >= 1)
        {
            hasRotated = true;
            //TODO: Call EventHandler
        }
    }
}
