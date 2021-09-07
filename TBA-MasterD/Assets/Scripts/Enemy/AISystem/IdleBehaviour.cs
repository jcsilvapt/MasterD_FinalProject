using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class IdleBehaviour : AIBehaviour {

    // Behaviour Settings
    private bool isActive;

    private Animator anim;
    private NavMeshAgent agent;
    private Transform target;

    private float idleTime;
    private float elapsedTime;

    private Transform enemyHead;

    public IdleBehaviour(MonoBehaviour self, AIStateMachine stateMachine, Transform enemyHead, float idleTime) : base(self, stateMachine, "Idle") {
        this.enemyHead = enemyHead;
        this.idleTime = idleTime;
    }

    public override void Init() {
        anim = self.GetComponent<Animator>();
        agent = self.GetComponent<NavMeshAgent>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void OnBehaviourEnd() {
        elapsedTime = 0.0f;
        isActive = false;
        anim.SetBool("iIdle", false);
    }

    public override void OnBehaviourStart() {
        isActive = true;
        anim.SetBool("iIdle", true);
        //TODO: Falta controlar o animador.... anim.setbool("Idle", true);
    }

    public override void OnUpdate() {
        if (isActive) {
            if (AIUtils_Fabio.HasVisionOfPlayer(enemyHead.transform, target, self.GetComponent<Enemy>().GetDistanceToView())) {
                stateMachine.HandleEvent(AIEvents.SeePlayer);
                return;
            }
            if (elapsedTime >= idleTime) {
                stateMachine.HandleEvent(AIEvents.NoLongerIdle);
            } else {
                elapsedTime += Time.deltaTime;
            }
        }
    }
}
