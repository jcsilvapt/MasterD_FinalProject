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

    public IdleBehaviour(MonoBehaviour self, AIStateMachine stateMachine, float idleTime) : base(self, stateMachine, "Idle") {
        this.idleTime = idleTime;
    }

    public override void Init() {
        anim = self.GetComponent<Animator>();
        agent = self.GetComponent<NavMeshAgent>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void InitBehaviour() {
        isActive = true;
    }


    public override void DisableBehaviour() {
        isActive = false;
    }

    public override void OnBehaviourEnd() {
        self.StopAllCoroutines();
    }

    public override void OnBehaviourStart() {
        Debug.Log("Idle Behaviour OnGoing...");

        self.StartCoroutine(IdleCooldown());

        //TODO: Falta controlar o animador.... anim.setbool("Idle", true);
    }

    public override void OnUpdate() {
        if(isActive) {
            if(AIUtils.HasVisionOfPlayer(self.transform, target)) {
                stateMachine.HandleEvent(AIEvents.SeePlayer);
                return;
            }
        }
    }

    private IEnumerator IdleCooldown() {
        yield return new WaitForSeconds(idleTime);

        stateMachine.HandleEvent(AIEvents.NoLongerIdle);
    }
}
