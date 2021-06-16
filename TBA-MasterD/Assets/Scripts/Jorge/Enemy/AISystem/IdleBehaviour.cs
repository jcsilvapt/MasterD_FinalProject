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

    public IdleBehaviour(MonoBehaviour self, AIStateMachine stateMachine, float idleTime) : base(self, stateMachine, "Idle") {
        this.idleTime = idleTime;
    }

    public override void Init() {
        anim = self.GetComponent<Animator>();
        agent = self.GetComponent<NavMeshAgent>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void OnBehaviourEnd() {
        Debug.Log("Idle Behaviour Ended");
        elapsedTime = 0.0f;
        isActive = false;
    }

    public override void OnBehaviourStart() {
        Debug.Log("Idle Behaviour Started");
        isActive = true;

        //TODO: Falta controlar o animador.... anim.setbool("Idle", true);
    }

    public override void OnUpdate() {
        if (isActive) {
            if (AIUtils_Fabio.HasVisionOfPlayer(self.transform, target, 5)) {
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
