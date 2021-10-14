using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, AIStateMachine, IDamage
{

    // References
    [Header("Components")]
    [Tooltip("material of the color that will change with hp")]
    [SerializeField] Material healthEmission;
    private Color healthColor; // color of the hp that will change with hits
    private float healthC; //percentage of color change for each hit
    private Animator animator;
    private Rigidbody rb;
    private GameObject character;
    private NavMeshAgent agent;
    [Tooltip("mesh of the object that has the health color material")]
    [SerializeField] GameObject himself;
    [SerializeField] GameObject redLight;
    private bool isShooting;
    [SerializeField] Transform enemyHead;
    [Tooltip("used to check if ammo is equal or below half")]
    [SerializeField] GameObject ammoPack;
    [SerializeField] GameObject healthPack;
    [SerializeField] GameObject healthSpawner;
    [SerializeField] GameObject ammoSpawner;


    [Header("Shooting Settings")]
    //objects for shooting
    public GameObject bullet;
    public GameObject bulletSpawn;
    public GameObject casing;
    public GameObject casingSpawn;
    public ParticleSystem muzzleFlash;
    public float timeToShoot = 1f;
    public float elapsedTime = 0;
    [SerializeField] AudioSource shootSoundSource;
    [SerializeField] AudioClip shootSound;


    [Header("Enemy Settings")]

    [SerializeField] float health;
    [Tooltip("Use to determine if this character is alive or not.")]
    [SerializeField] bool isAlive = true;
    [Tooltip("Used to see if there are allies nearby to attack target")]
    [SerializeField] bool canAlertClosebyEnemies = false;

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
    public bool hasAlertedOther = false;

    private AIStates currentState;
    private AIBehaviour currentBehaviour;
    private AIBehaviour[] behaviours;

    [Header("Developer Settings")]
    [Tooltip("Enable this for movement test only.")]
    [SerializeField] bool enableTestMovement = false;

    private bool canShoot = false;
    


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
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("PlayerParent").transform;
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
            CheckIfCanShoot();
        }
        else
        {
            health = 0;
            TakeDamage();
        }
        
    }

    #region PUBLIC METHODS

    public void SetIsAlive(bool value) {
        isAlive = value;
    }

    #endregion

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
        //put bool false <---------------------------------
        EnableNextBehaviour(nextState);

    }
    public void CheckSurroundingEnemies() //see if there are enemies nearby and activate behaviours to help them attack the player
    {
        if (canAlertClosebyEnemies == true)
        {
            if (hasAlertedOther == false)
            {
                Collider[] hitColliders = Physics.OverlapBox(this.transform.position, new Vector3(7, 1, 7));

                foreach (Collider Ally in hitColliders)
                {
                    if (Ally.gameObject.tag == "Enemy")
                    {

                        if (Ally.transform.GetComponent<Enemy>().currentState != AIStates.GotHit || Ally.transform.GetComponent<Enemy>().currentState != AIStates.Chase)
                        {
                            hasAlertedOther = true;
                            Ally.transform.GetComponent<Enemy>().HandleEvent(AIEvents.GotAttacked);
                        }
                    }
                }
            }
        }
    }


    public float GetDistanceToView()
    {
        return distanceToViewTarget;
    }

    public float GetDistanceToShoot() {
        return distanceToShoot;
    }
    
    #endregion

    #region Shooting

    private void CheckIfCanShoot()
    {
        if (elapsedTime >= timeToShoot)
        {
            elapsedTime = 0f;
            canShoot = true;
        }
        else
        {
            canShoot = false;
            elapsedTime += Time.deltaTime;
        }
    }

    public void Shoot() //shooting and timer
    {
        if (canShoot)
        {            
            RaycastHit hit;
            if (Physics.Raycast(bulletSpawn.transform.position, ((target.position + Vector3.up) - bulletSpawn.transform.position) + SetSpray(), out hit))
            {
                if (hit.transform.GetComponent<IDamage>() != null)
                {
                    hit.transform.GetComponent<IDamage>().TakeDamage();
                }
            }
        }
    }

    public void SetShooting(bool iShoot)
    {
        isShooting = iShoot;
    }

    private void ShootSound() // used for shooting sounds in an event on the animation
    {
        shootSoundSource.PlayOneShot(shootSoundSource.clip);
        muzzleFlash.Play();
    }

    #endregion

    #region Animation, Alive Checker and Drop Packs
    public void TakeDamage()
    {
        // Get all enemies closeby and activate the behaviour...
        if (isShooting == false && isAlive)
        {
            HandleEvent(AIEvents.GotAttacked);
        }
        health -= 10;
        healthC = healthC + 0.1f;
        if (health <= 0) // Is Dead and does this
        {
            SetKinematic(false);
            animator.enabled = false;
            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            this.gameObject.layer = 13;
            redLight.SetActive(false);
            DisableAgent();
            PackDropper();
            Debug.Log("me dies");
            return;
        }
        CheckSurroundingEnemies();
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

    private void PackDropper()
    {
        float health = target.GetComponent<charController>().GetHealth();
        int currentBullets = (int)target.GetComponent<charController>().GetCurrentWeaponBullets().y;
        int currentMaximumBullets = (int)target.GetComponent<charController>().GetCurrentWeaponBullets().x;

        Debug.Log("health: " + health + ", MaximumBullets: " + currentMaximumBullets + ", CurrentBullets: " + currentBullets);

        if (currentBullets <= currentMaximumBullets / 2 && health <= 50)
        {
            Instantiate(healthPack, healthSpawner.transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            Instantiate(ammoPack, ammoSpawner.transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            Debug.Log("Droping Health & Ammo");
        }
        else if (currentBullets <= currentMaximumBullets / 2)
        {
            Instantiate(ammoPack, ammoSpawner.transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            Debug.Log("Dropping Ammo");
        }
        else if (health <= 50)
        {
            Instantiate(healthPack, healthSpawner.transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            Debug.Log("Dropping Health");
        }
        else
        {

        }
    }
    #endregion

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
        Physics.IgnoreLayerCollision(12, 9);
    }

    #endregion

    #region Fabio Shooting Edition

    private Vector3 SetSpray()
    {
        Vector3 spray = new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f));

        return spray;
    }

    #endregion
}
