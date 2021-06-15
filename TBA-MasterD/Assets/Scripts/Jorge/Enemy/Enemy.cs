using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, AIStateMachine {

    // References
    private Animator animator;
    private Rigidbody rb;
    private GameObject character;

    [Header("Enemy Settings")]

    [SerializeField] float health;
    [Tooltip("Use to determine if this character is alive or not.")]
    [SerializeField] bool isAlive = true;

    [Header("AI Settings")]

    [Tooltip("Usually needs to be enable...")]
    [SerializeField] bool enableAISystem = true;
    [Tooltip("Set how long the character will stay in IdleMode")]
    [Range(1.0f, 10.0f)]
    [SerializeField] float idleTime = 1.0f;


    private AIStates currentState;
    private AIBehaviour currentBehaviour;
    private AIBehaviour[] behaviours;


    Dictionary<AIEvents, AIStates> nextEvent = new Dictionary<AIEvents, AIStates> {
        [AIEvents.NoLongerIdle] = AIStates.Patrol,
        [AIEvents.SeePlayer] = AIStates.Chase,
        [AIEvents.ReachedDestination] = AIStates.Idle,
        [AIEvents.InRange] = AIStates.Attack,
        [AIEvents.RangeToFar] = AIStates.Chase,
        [AIEvents.LostPlayer] = AIStates.Idle,
        [AIEvents.GotAttacked] = AIStates.GotHit,
    };


    private void Start() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        BehaviourRegistration();
    }

    private void Update() {
        if (isAlive && enableAISystem) {
            currentBehaviour.OnUpdate();
        } else {
            //TODO: 
        }
    }

    #region AI System

    /// <summary>
    /// Register All Behaviours (expand when new behaviours is created...)
    /// </summary>
    private void BehaviourRegistration() {
        if (isAlive && enableAISystem) {
            behaviours = new AIBehaviour[] {
                new IdleBehaviour(this, this, idleTime),
                new PatrolBehaviour(this, this),
                //New Behaviours here
            };

            foreach (AIBehaviour b in behaviours) {
                b.Init();
            }

            EnableNextBehaviour(AIStates.Idle);

        }
    }

    /// <summary>
    /// Function that Enables the next State to be Active
    /// </summary>
    /// <param name="newState"></param>
    private void EnableNextBehaviour(AIStates newState) {
        currentState = newState;
        currentBehaviour = behaviours[(int)currentState];
        currentBehaviour.OnBehaviourStart();
    }

    /// <summary>
    /// State Machine
    /// </summary>
    /// <param name="aiEvent"></param>
    public void HandleEvent(AIEvents aiEvent) {
        // Disables Current Behaviour
        currentBehaviour.OnBehaviourEnd();

        AIStates nextState = AIStates.Idle;

        switch (currentState) {
            case AIStates.Idle:
                nextState = nextEvent[aiEvent];
                break;
            case AIStates.Patrol:
                nextState = nextEvent[aiEvent];
                break;
            case AIStates.Chase:
                nextState = nextEvent[aiEvent];
                break;
            case AIStates.Attack:
                nextState = nextEvent[aiEvent];
                break;
            case AIStates.GotHit:
                nextState = nextEvent[aiEvent];
                break;
            default:
                break;
        }

        EnableNextBehaviour(nextState);

    }


    #endregion
}
