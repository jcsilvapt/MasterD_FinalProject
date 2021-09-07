using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, AIStateMachine, IDamage
{

    // References
    private Animator animator;
    private Rigidbody rb;
    private GameObject character;
    private NavMeshAgent agent;
    public Material healthEmission; // material of the color that will change with hp
    private Color healthColor; // color of the hp that will change with hits
    public float healthC;
    public GameObject himself; //mesh of the object that has the health color material
    public GameObject redLight;
    private bool isShooting;
    private Transform enemyHead;

    [Header("Shooting Settings")]
    //objects for shooting
    public GameObject bullet;
    public GameObject bulletSpawn;
    public GameObject casing;
    public GameObject casingSpawn;
    public ParticleSystem muzzleFlash;
    public float timeToShoot = 0.5f;
    public float elapsedTime;


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
    [SerializeField] float distanceToShoot;
    [Tooltip("Define here the patrol points of this AI System")]
    [SerializeField] Transform[] patrolWayPoints;

    private AIStates currentState;
    private AIBehaviour currentBehaviour;
    private AIBehaviour[] behaviours;

    [Header("Developer Settings")]
    [Tooltip("Enable this for movement test only.")]
    [SerializeField] bool enableTestMovement = false;


    Dictionary<AIEvents, AIStates> nextEvent = new Dictionary<AIEvents, AIStates>
    {
        [AIEvents.NoLongerIdle] = AIStates.Patrol,
        [AIEvents.SeePlayer] = AIStates.Chase,
        [AIEvents.ReachedDestination] = AIStates.Idle,
        [AIEvents.InRange] = AIStates.Attack,
        [AIEvents.RangeToFar] = AIStates.Chase,
        [AIEvents.LostPlayer] = AIStates.RandomSearch,
        [AIEvents.GotAttacked] = AIStates.GotHit,
    };


    private void Start()
    {
        enemyHead = transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        healthEmission = himself.GetComponent<SkinnedMeshRenderer>().material;
        SetKinematic(true);
        BehaviourRegistration();
    }

    private void Update()
    {
        healthEmission.SetColor("_EmissionColor", healthColor * 3); // access to emission color of the health material

        healthColor = Color.Lerp(Color.green, Color.red * 3, healthC); //gradient between two color for the enemy health

        if (isAlive && enableAISystem)
        {
            currentBehaviour.OnUpdate();
        }
        else
        {
            //TODO: º+p
        }
        Debug.Log(enemyHead);
    }

    public float GetDistanceToView()
    {
        return distanceToViewTarget;
    }


    #region AI System

    /// <summary>
    /// Register All Behaviours (expand when new behaviours is created...)
    /// </summary>
    private void BehaviourRegistration()
    {
        if (isAlive && enableAISystem)
        {
            behaviours = new AIBehaviour[] {
                new IdleBehaviour(this, this, enemyHead, idleTime),
                new PatrolBehaviour(this, this, enemyHead, patrolWayPoints),
                new ChaseBehaviour(this,this, enemyHead),
                new AttackBehaviour(this, this, enemyHead, distanceToShoot),
                new GotHitBehaviour(this, this, enemyHead),
                new RandomSearchBehaviour(this, this, enemyHead)
                // New Behaviours GOES HERE
            };

            foreach (AIBehaviour b in behaviours)
            {
                b.Init();
            }

            EnableNextBehaviour(AIStates.Idle);

        }
    }

    /// <summary>
    /// Function that Enables the next State to be Active
    /// </summary>
    /// <param name="newState"></param>
    private void EnableNextBehaviour(AIStates newState)
    {
        currentState = newState;
        currentBehaviour = behaviours[(int)currentState];
        currentBehaviour.OnBehaviourStart();
    }

    /// <summary>
    /// State Machine
    /// </summary>
    /// <param name="aiEvent"></param>
    public void HandleEvent(AIEvents aiEvent)
    {
        // Disables Current Behaviour
        currentBehaviour.OnBehaviourEnd();

        AIStates nextState = AIStates.Idle;

        switch (currentState)
        {
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

    #region Shooting

    public void Shoot() //shooting and timer
    {
        if (elapsedTime >= timeToShoot)
        {
            bulletSpawn.transform.LookAt(target.transform.position);
            Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation); // instantiate bullet
            Instantiate(casing, casingSpawn.transform.position, casingSpawn.transform.rotation); // instantiate bullet casing
            muzzleFlash.Play();
            Debug.Log("Just Shoot");
            elapsedTime = 0f;
        }
        else
        {
            elapsedTime += Time.deltaTime;
        }

    }
    public void SetShooting(bool iShoot)
    {
        isShooting = iShoot;
    }
    #endregion
    public void TakeDamage()
    {
        if (isShooting == false)
        {
            HandleEvent(AIEvents.GotAttacked);
        }
        health -= 10;
        healthC = healthC + 0.1f;

        if (health <= 0)
        {
            SetKinematic(false);
            animator.enabled = false;
            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            redLight.SetActive(false);
            DisableAgent();
            Debug.Log("me dies");
        }

    }

    private void OnAnimatorMove()
    {
        if (Time.deltaTime != 0)
        {
            agent.speed = (animator.deltaPosition / Time.deltaTime).magnitude;
        }
    }

    private void DisableAgent()
    {
        if (behaviours != null)
        {
            foreach (AIBehaviour b in behaviours)
            {
                b.OnBehaviourEnd();
                //b.KillBehaviour();
            }
        }
    }


    #region Ragdoll
    private void SetKinematic(bool value)
    {
        Rigidbody[] bodyParts = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in bodyParts)
        {
            rb.isKinematic = value;
        }

        Collider[] bodyColisions = GetComponentsInChildren<Collider>();
        foreach (Collider col in bodyColisions)
        {
            col.enabled = !value;
        }
        GetComponent<Collider>().enabled = true;
    }


    #endregion
}
