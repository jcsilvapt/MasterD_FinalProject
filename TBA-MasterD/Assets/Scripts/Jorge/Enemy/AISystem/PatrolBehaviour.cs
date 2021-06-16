using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehaviour : AIBehaviour {

    private bool isActive;

    private Animator anim;
    private NavMeshAgent agent;
    private Transform target;

    // Patrol Logic
    private Transform[] waypoints;
    private int currentWayPoint = -1;


    public PatrolBehaviour(MonoBehaviour self, AIStateMachine stateMachine, Transform[] waypoints) : base(self, stateMachine, "Patrol") {
        this.waypoints = waypoints;
    }

    public override void Init() {
        anim = self.GetComponent<Animator>();
        agent = self.GetComponent<NavMeshAgent>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

    }

    public override void OnBehaviourEnd() {
        Debug.Log("Patrol Behaviour Ended");
        isActive = false;

    }

    public override void OnBehaviourStart() {
        Debug.Log("Patrol Behaviour Started");
        isActive = true;
        NextWayPoint();

    }

    public override void OnUpdate() {
        if (isActive) {
            if (AIUtils.HasVisionOfPlayer(self.transform, target)) {
                //stateMachine.HandleEvent(AIEvents.SeePlayer);
                Debug.Log("I SAW THE FUCKING PLAYER");
                return;
            } else {
                if (!agent.pathPending && agent.remainingDistance < 0.1f) {
                    stateMachine.HandleEvent(AIEvents.ReachedDestination);
                    return;
                }
            }
        }
    }

    private void NextWayPoint() {
        if (waypoints != null && waypoints.Length > 0) {
            currentWayPoint = (currentWayPoint + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWayPoint].position);
        }
    }
}
