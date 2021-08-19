using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehaviour : AIBehaviour {

    private bool isActive;

    private Animator anim;
    private NavMeshAgent agent;
    private Transform target;
    private bool hasWaypoints;

    // Patrol Logic
    private Transform[] waypoints;
    private int currentWayPoint = -1;
    private Vector3 initialPosition;


    public PatrolBehaviour(MonoBehaviour self, AIStateMachine stateMachine, Transform[] waypoints) : base(self, stateMachine, "Patrol") {
        this.waypoints = waypoints;
        if (this.waypoints.Length == 0) {
            hasWaypoints = false;
        } else {
            hasWaypoints = true;
        }
    }

    public override void Init() {
        anim = self.GetComponent<Animator>();
        agent = self.GetComponent<NavMeshAgent>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
        initialPosition = self.transform.position;

    }

    private bool IsFarFromOrigin() {
        float distance = Vector3.Distance(self.transform.position, initialPosition);
        if (distance > 2f) {
            return true;
        }
        return false;
    }

    public override void OnBehaviourEnd() {
        isActive = false;
        anim.SetBool("iWalk", false);
    }

    public override void OnBehaviourStart() {
        if (hasWaypoints) {
            isActive = true;
            NextWayPoint();
            anim.SetBool("iWalk", true);
        } else {
            if (IsFarFromOrigin()) {
                anim.SetBool("iWalk", true);
                agent.SetDestination(initialPosition);
                isActive = true;
            } else {
                stateMachine.HandleEvent(AIEvents.ReachedDestination);
            }
        }
    }

    public override void OnUpdate() {
        if (isActive) {
            if (AIUtils_Fabio.HasVisionOfPlayer(self.transform, target, self.GetComponent<Enemy>().GetDistanceToView())) {
                stateMachine.HandleEvent(AIEvents.SeePlayer);
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
