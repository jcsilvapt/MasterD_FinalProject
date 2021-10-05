using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fabio_AwareBehaviour : Fabio_AIBehaviour
{
    // Behaviour Settings
    private bool isActive;

    #region References

    private Animator animator;
    private NavMeshAgent agent;
    private Transform target;

    private Transform enemyHead;
    private Fabio_AIManager aiManager;

    #endregion

    public Fabio_AwareBehaviour(MonoBehaviour self, Fabio_AIStateMachine stateMachine, Transform enemyHead, Fabio_AIManager aiManager) : base(self, stateMachine, "Aware")
    {
        this.enemyHead = enemyHead;
        this.aiManager = aiManager;
    }

    public override void Init()
    {
        animator = self.GetComponent<Animator>();
        agent = self.GetComponent<NavMeshAgent>();

        target = GameObject.FindGameObjectWithTag("PlayerParent").transform;
    }

    public override void OnBehaviourStart()
    {
        isActive = true;
        animator.SetBool("IsAware", true);
    }

    public override void OnBehaviourEnd()
    {
        isActive = false;
        animator.SetBool("IsAware", false);
    }

    public override void OnUpdate()
    {
        if (isActive)
        {
            if (AIUtils_Fabio.HasVisionOfPlayer(enemyHead, target, 50))
            {
                self.GetComponent<Fabio_EnemySecondLevel>().SetIsSeeingPlayer(true);
                stateMachine.HandleEvent(Fabio_AIEvents.SawPlayer);
            }
        }
    }
}
