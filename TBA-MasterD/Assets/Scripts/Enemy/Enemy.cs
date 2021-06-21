using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, AIStateMachine {

    // References
    private Animator animator;
    private Rigidbody rb;
    private GameObject character;
    private NavMeshAgent agent;

    [Header("Enemy Settings")]

    [SerializeField] float health;
    [Tooltip("Use to determine if this character is alive or not.")]
    [SerializeField] bool isAlive = true;

    [Header("AI Settings")]

    [Tooltip("Usually needs to be enable...")]
    [SerializeField] bool enableAISystem = true;
    [SerializeField] Transform target;
    [Tooltip("Set how long the character will stay in IdleMode")]
    [Range(1.0f, 10.0f)]
    [SerializeField] float idleTime = 1.0f;
    [Range(10.0f, 200.0f)]
    [SerializeField] float distanceToViewTarget = 10.0f;
    [Tooltip("Define here the patrol points of this AI System")]
    [SerializeField] Transform[] patrolWayPoints;

    private AIStates currentState;
    private AIBehaviour currentBehaviour;
    private AIBehaviour[] behaviours;

    [Header("Developer Settings")]
    [Tooltip("Enable this for movement test only.")]
    [SerializeField] bool enableTestMovement = false;


    Dictionary<AIEvents, AIStates> nextEvent = new Dictionary<AIEvents, AIStates> {
        [AIEvents.NoLongerIdle] = AIStates.Patrol,
        [AIEvents.SeePlayer] = AIStates.Chase,
        [AIEvents.ReachedDestination] = AIStates.Idle,
        [AIEvents.InRange] = AIStates.Attack,
        [AIEvents.RangeToFar] = AIStates.Chase,
        [AIEvents.LostPlayer] = AIStates.RandomSearch,
        [AIEvents.GotAttacked] = AIStates.GotHit,
    };


    private void Start() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        BehaviourRegistration();


    }

    private void Update() {
        if (isAlive && enableAISystem) {
            currentBehaviour.OnUpdate();
            if (Input.GetKeyDown(KeyCode.E))
            {
                HandleEvent(AIEvents.GotAttacked); //Mudar para o Random Search.
            }
        } else {
            //TODO: 
        }
    }

    public float GetDistanceToView() {
        return distanceToViewTarget;
    }


    #region AI System

    /// <summary>
    /// Register All Behaviours (expand when new behaviours is created...)
    /// </summary>
    private void BehaviourRegistration() {
        if (isAlive && enableAISystem) {
            behaviours = new AIBehaviour[] {
                new IdleBehaviour(this, this, idleTime),
                new PatrolBehaviour(this, this, patrolWayPoints),
                new ChaseBehaviour(this,this),
                new AttackBehaviour(this, this),
                new GotHitBehaviour(this, this),
                new RandomSearchBehaviour(this, this)
                // New Behaviours GOES HERE
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
            case AIStates.RandomSearch:
                nextState = nextEvent[aiEvent];
                break;
            default:
                break;
        }

        EnableNextBehaviour(nextState);

    }


    #endregion
}
