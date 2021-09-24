using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fabio_IdleBehaviour : Fabio_AIBehaviour
{
    // Behaviour Settings
    private bool isActive;

    #region References

    private Animator animator;
    private NavMeshAgent agent;
    private Transform target;

    private Transform enemyHead;

    #endregion

    public Fabio_IdleBehaviour(MonoBehaviour self, Fabio_AIStateMachine stateMachine, Transform enemyHead) : base(self, stateMachine, "Idle")
    {
        this.enemyHead = enemyHead;
    }

    public override void Init()
    {
        animator = self.GetComponent<Animator>();
        agent = self.GetComponent<NavMeshAgent>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void OnBehaviourStart()
    {
        isActive = true;
        animator.SetBool("IsIdle", true);

        Debug.Log("Started " + GetName());
    }

    public override void OnBehaviourEnd()
    {
        isActive = false;
        animator.SetBool("IsIdle", false);

        Debug.Log("Ended " + GetName());
    }

    public override void OnUpdate()
    {
        return;
    }
}
