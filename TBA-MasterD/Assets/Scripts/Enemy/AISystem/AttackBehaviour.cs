using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : AIBehaviour {


    public AttackBehaviour(MonoBehaviour self, AIStateMachine stateMachine) : base (self, stateMachine, "Attack") {

    }

    public override void Init() {

    }

    public override void OnBehaviourEnd() {

    }

    public override void OnBehaviourStart() {
        stateMachine.HandleEvent(AIEvents.ReachedDestination);
    }

    public override void OnUpdate() {

    }
}
