using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehaviour : AIBehaviour {

    private bool isActive;

    private Animator anim;
    private NavMeshAgent agent;
    private Transform target;


    public PatrolBehaviour(MonoBehaviour self, AIStateMachine stateMachine) : base(self, stateMachine, "Patrol") {

    }


    public override void Init() {
        anim = self.GetComponent<Animator>();
        agent = self.GetComponent<NavMeshAgent>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        InitBehaviour();
    }

    public override void InitBehaviour() {
        isActive = true;
    }
    public override void DisableBehaviour() {
        isActive = false;
    }

    public override void OnBehaviourEnd() {
        //TODO:
    }

    public override void OnBehaviourStart() {
        Debug.Log("Patrol Activated");
    }

    public override void OnUpdate() {
        if(isActive) {
            if(AIUtils.HasVisionOfPlayer(self.transform, target)) {
                stateMachine.HandleEvent(AIEvents.SeePlayer);
                return;
            }
            stateMachine.HandleEvent(AIEvents.ReachedDestination);
            return;
        }
    }
}
