using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : AIBehaviour
{

    public bool isActive;
    public Vector3 lastPlayerSeen;
    public bool playerIsVisible = false;
    public Transform target;
    public NavMeshAgent navAgent;
    private Animator anim;

    private Transform enemyHead;

    private Vector3 distance;
    public ChaseBehaviour(MonoBehaviour self, AIStateMachine stateMachine, Transform selfHead) : base(self, stateMachine, "Chase")
    {
        this.enemyHead = selfHead;
    }


    public override void Init()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = self.GetComponent<NavMeshAgent>();
        anim = self.GetComponent<Animator>();
    }

    public override void OnBehaviourEnd()
    {
        Debug.Log("Chase Ended");
        navAgent.ResetPath();
        isActive = false;
        anim.SetBool("iChase", false);
    }

    public override void OnBehaviourStart()
    {
        Debug.Log("Lets start Chase him!");
        isActive = true;
        ChasePlayer();
        anim.SetBool("iChase", true);
    }

    public override void OnUpdate()
    {
        if (isActive)
        {
            if (AIUtils_Fabio.HasVisionOfPlayer(enemyHead.transform, target, self.GetComponent<Enemy>().GetDistanceToView())) //checks if player distance is enought to be seen
            {

                if (AIUtils_Tiago.IsChasingPlayer(self.transform, target, 5)) //checks if the distance is enought to shoot player
                {
                    stateMachine.HandleEvent(AIEvents.InRange);
                    return;
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
        navAgent.SetDestination(target.position);
    }

}
