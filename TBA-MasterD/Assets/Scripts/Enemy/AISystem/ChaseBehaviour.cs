using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : AIBehaviour
{

    public bool isActive;

    public Transform target;
    public NavMeshAgent navAgent;

    private Vector3 distance;
    public ChaseBehaviour(MonoBehaviour self, AIStateMachine stateMachine) : base(self, stateMachine, "Chase")
    {

    }


    public override void Init()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = self.GetComponent<NavMeshAgent>();
    }

    public override void OnBehaviourEnd()
    {
        Debug.Log("Chase Ended");
        navAgent.ResetPath();
        isActive = false;
    }

    public override void OnBehaviourStart()
    {
        Debug.Log("Lets start Chase him!");
        isActive = true;
        ChasePlayer();
    }

    public override void OnUpdate()
    {
        if (isActive)
        {
            if (AIUtils_Fabio.HasVisionOfPlayer(self.transform, target, 25)) //checks if player is visible
            {
                Debug.Log("I saw the runner");
                if (AIUtils_Tiago.IsChasingPlayer(self.transform, target, 10)) //checks his distance 
                {
                    //stateMachine.HandleEvent(AIEvents.InRange);
                    Debug.Log("Im Chasing the player");
                }
                else
                {
                    ChasePlayer();
                }
            }
            else
            {
                stateMachine.HandleEvent(AIEvents.LostPlayer);
                return;
            }
        }
    }

    private void ChasePlayer()
    {
        // float distance = Vector3.Distance(target.position, self.transform.position);

        navAgent.SetDestination(target.position);

    }
}
