using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSearchBehaviour : AIBehaviour
{
    public RandomSearchBehaviour(MonoBehaviour self, AIStateMachine stateMachine) : base(self, stateMachine, "RandomSearch")
    {

    }

    public override void Init()
    {

    }

    public override void OnBehaviourEnd()
    {
        Debug.Log("Random Search Behaviour Ended");
    }

    public override void OnBehaviourStart()
    {
        Debug.Log("Random Search Behaviour Started");
    }

    public override void OnUpdate()
    {

    }
}
