using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : AIBehaviour
{

    [SerializeField] bool isActive;
    [SerializeField] Vector3 lastPlayerSeen;
    [SerializeField] bool playerIsVisible = false;
    [SerializeField] Transform target;
    [SerializeField] NavMeshAgent navAgent;
    private Animator anim;

    private Transform enemyHead;


    private Vector3 distance;
    public ChaseBehaviour(MonoBehaviour self, AIStateMachine stateMachine, Transform selfHead) : base(self, stateMachine, "Chase")
    {
        this.enemyHead = selfHead;
    }


    public override void Init()
    {
        target = GameObject.FindGameObjectWithTag("PlayerParent").transform;
        navAgent = self.GetComponent<NavMeshAgent>();
        anim = self.GetComponent<Animator>();
    }

    public override void OnBehaviourEnd()
    {
        navAgent.ResetPath();
        isActive = false;
        anim.SetBool("iChase", false);
    }

    public override void OnBehaviourStart()
    {
        enemyHead.LookAt(target);
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
                if (AIUtils_Tiago.IsChasingPlayer(self.transform, target, self.GetComponent<Enemy>().GetDistanceToShoot())) //checks if the distance is enought to shoot player
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
                if (!navAgent.pathPending && navAgent.remainingDistance < 0.1f)
                {
                    stateMachine.HandleEvent(AIEvents.LostPlayer);
                    return;
                }
            }
        }
    }
    private void ChasePlayer()
    {
        enemyHead.LookAt(target);
        navAgent.SetDestination(target.position);
    }

}
