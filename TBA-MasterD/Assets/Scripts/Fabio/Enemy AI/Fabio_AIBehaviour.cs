using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fabio_AIBehaviour
{
    protected MonoBehaviour self;
    protected Fabio_AIStateMachine stateMachine;
    private string behaviourName;

    /// <summary>
    /// AIBehaviour Constructor
    /// </summary>
    /// <param name="self">Self Monobehaviour</param>
    /// <param name="stateMachine">Current StateMachine</param>
    /// <param name="behaviourName">Behaviour Name</param>
    public Fabio_AIBehaviour(MonoBehaviour self, Fabio_AIStateMachine stateMachine, string behaviourName)
    {
        this.self = self;
        this.stateMachine = stateMachine;
        this.behaviourName = behaviourName;
    }

    /// <summary>
    /// Function that returns the name of this Behaviour.
    /// </summary>
    /// <returns>Name of the behaviour</returns>
    public string GetName()
    {
        return behaviourName;
    }

    /// <summary>
    /// Function to Initialize the behaviour
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// All Behaviour Logic Goes here (Works like normal Update).
    /// </summary>
    public abstract void OnUpdate();

    /// <summary>
    /// Function that enable this Behaviour;
    /// </summary>
    public abstract void OnBehaviourStart();

    /// <summary>
    /// Function that disable this Behaviour;
    /// </summary>
    public abstract void OnBehaviourEnd();
}

