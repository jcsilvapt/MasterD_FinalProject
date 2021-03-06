using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fabio_EnemySecondLevel : MonoBehaviour, Fabio_AIStateMachine, IDamage
{
    // References
    [Header("Components")]
    [Tooltip("material of the color that will change with hp")]
    [SerializeField] Material healthEmission;
    private Color healthColor; // color of the hp that will change with hits
    private float healthC; //percentage of color change for each hit
    private Animator animator;
    private Rigidbody rb;
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
    public GameObject muzzleFlash;
    [SerializeField] AudioSource shootSoundSource;
    [SerializeField] AudioClip shootSound;


    [Header("Enemy Settings")]

    [SerializeField] float health;
    [Tooltip("Use to determine if this character is alive or not.")]
    [SerializeField] bool isAlive = true;

    [Header("AI Settings")]

    [Tooltip("Usually needs to be enable...")]
    [SerializeField] bool enableAISystem = true;
    [SerializeField] Transform target;

    private Fabio_AIStates currentState;
    private Fabio_AIBehaviour currentBehaviour;
    private Fabio_AIBehaviour[] behaviours;

    [Header("Specified AI Manager Reference")]
    [SerializeField] private Fabio_AIManager aiManager;

    Dictionary<Fabio_AIEvents, Fabio_AIStates> nextEvent = new Dictionary<Fabio_AIEvents, Fabio_AIStates>
    {
        [Fabio_AIEvents.BecomeAware] = Fabio_AIStates.Aware,
        [Fabio_AIEvents.SawPlayer] = Fabio_AIStates.ReadyToFire,
        [Fabio_AIEvents.LostPlayer] = Fabio_AIStates.Aware,
        [Fabio_AIEvents.StartChasing] = Fabio_AIStates.Chase,
        [Fabio_AIEvents.InShootingRange] = Fabio_AIStates.Shoot,
        [Fabio_AIEvents.HitPlayer] = Fabio_AIStates.MissAllShots,
        [Fabio_AIEvents.FinishedShooting] = Fabio_AIStates.ReadyToFire
    };

    #region Edits

    private bool isSeeingThePlayer;

    #endregion

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
        }
    }


    #region AI System

    /// <summary>
    /// Register All Behaviours (expand when new behaviours is created...)
    /// </summary>
    private void BehaviourRegistration()
    {
        if (isAlive && enableAISystem)
        {
            behaviours = new Fabio_AIBehaviour[] {

                new Fabio_IdleBehaviour(this, this, enemyHead),
                new Fabio_AwareBehaviour(this, this, enemyHead, aiManager),
                new Fabio_ReadyToFireBehaviour(this, this, enemyHead, aiManager),
                new Fabio_ShootBehaviour(this, this, bulletSpawn.transform, aiManager, shootSoundSource, muzzleFlash),
                new Fabio_MissAllShots(this, this, bulletSpawn.transform, 0, aiManager, shootSoundSource, muzzleFlash),
                new Fabio_ChaseBehaviour(this, this, enemyHead, aiManager)
            };

            foreach (Fabio_AIBehaviour b in behaviours)
            {
                b.Init();
            }

            EnableNextBehaviour(Fabio_AIStates.Idle);
        }
    }

    /// <summary>
    /// Function that Enables the next State to be Active
    /// </summary>
    /// <param name="newState"></param>
    private void EnableNextBehaviour(Fabio_AIStates newState)
    {
        currentState = newState;
        currentBehaviour = behaviours[(int)currentState];
        currentBehaviour.OnBehaviourStart();
    }

    /// <summary>
    /// State Machine
    /// </summary>
    /// <param name="aiEvent"></param>
    public void HandleEvent(Fabio_AIEvents aiEvent)
    {
        // Disables Current Behaviour
        currentBehaviour.OnBehaviourEnd();

        Fabio_AIStates nextState = Fabio_AIStates.Idle;

        switch (currentState)
        {
            case Fabio_AIStates.Idle:
                nextState = nextEvent[aiEvent];
                break;
            case Fabio_AIStates.Aware:
                nextState = nextEvent[aiEvent];
                break;
            case Fabio_AIStates.ReadyToFire:
                nextState = nextEvent[aiEvent];
                break;
            case Fabio_AIStates.Shoot:
                nextState = nextEvent[aiEvent];
                break;
            case Fabio_AIStates.MissAllShots:
                nextState = nextEvent[aiEvent];
                break;
            case Fabio_AIStates.Chase:
                nextState = nextEvent[aiEvent];
                break;
        }
        //put bool false <---------------------------------
        EnableNextBehaviour(nextState);

    }
    #endregion

    #region Animation and Alive Checker
    public void TakeDamage()
    {
        health -= 10;
        healthC = healthC + 0.1f;

        if (health <= 0) // Is Dead and does this
        {
            SetKinematic(false);
            animator.enabled = false;
            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            gameObject.layer = 13;
            redLight.SetActive(false);
            PackDropper();
            DisableAgent();
            muzzleFlash.SetActive(false);
            aiManager.UnitKilled(this);
        }

    }

    private void OnAnimatorMove()
    {
        if (Time.deltaTime != 0)
        {
            if (animator != null) {
                agent.speed = (animator.deltaPosition / Time.deltaTime).magnitude;
            }
        }
    }

    private void DisableAgent()
    {
        if (behaviours != null)
        {

            foreach (Fabio_AIBehaviour b in behaviours)
            {
                b.OnBehaviourEnd();
                //b.KillBehaviour();
            }
        }
    }

    private void PackDropper()
    {
        float health = target.GetComponent<charController>().GetHealth();

        #region Fabio Edit

        int currentBullets = target.GetComponent<charController>().GetWeaponsAmmunition()[0];
        int currentMaximumBullets = target.GetComponent<charController>().GetWeaponsAmmunition()[1];

        #endregion

        if (currentBullets <= currentMaximumBullets / 2 && health <= 50)
        {
            Instantiate(healthPack, healthSpawner.transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            Instantiate(ammoPack, ammoSpawner.transform.position, Quaternion.Euler(new Vector3(-90, 0, 0))); 
        }
        else if (currentBullets <= currentMaximumBullets / 2)
        {
            Instantiate(ammoPack, ammoSpawner.transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        }
        else if (health <= 50)
        {
            Instantiate(healthPack, healthSpawner.transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            Debug.Log("Dropping Health");
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
            col.gameObject.layer = 13;
        }
        GetComponent<Collider>().enabled = true;

        Physics.IgnoreLayerCollision(13, 9);
    }

    #endregion

    #region Fabio Changes
    
    public float GetEnemyHealth()
    {
        return health;
    }
    
    public bool CanSeeThePlayer()
    {
        return AIUtils_Fabio.HasVisionOfPlayer(enemyHead, target, 50);
    }

    public string GetCurrentBehaviour()
    {
        return currentBehaviour.GetName();
    }

    public void SetAware()
    {
        HandleEvent(Fabio_AIEvents.BecomeAware);
    }

    public void SetShooting()
    {
        HandleEvent(Fabio_AIEvents.InShootingRange);
    }

    public void SetMissAllShots()
    {
        int numberOfShots = 0;

        if (currentBehaviour.GetName() == "Shoot")
        {
            numberOfShots = ((Fabio_ShootBehaviour)currentBehaviour).GetNumberOfShots();
        }

        HandleEvent(Fabio_AIEvents.HitPlayer);
        ((Fabio_MissAllShots) currentBehaviour).SetAllShots(numberOfShots);
    }

    public void SetChasing()
    {
        HandleEvent(Fabio_AIEvents.StartChasing);
    }

    public void SetIsSeeingPlayer(bool hasVision)
    {
        isSeeingThePlayer = hasVision;
    }

    public bool GetIsSeeingThePlayer()
    {
        return isSeeingThePlayer;
    }

    #endregion
}
