using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fabio_ShootBehaviour : Fabio_AIBehaviour
{
    // Behaviour Settings
    private bool isActive;

    #region References

    private Animator animator;
    private NavMeshAgent agent;
    private Transform target;

    private Transform bulletSpawn;
    private Fabio_AIManager aiManager;

    #endregion

    private int numberOfShots;

    private float timeBetweenShots;
    private float currentTimerBetweenShots;

    public Fabio_ShootBehaviour(MonoBehaviour self, Fabio_AIStateMachine stateMachine, Transform bulletSpawn, Fabio_AIManager aiManager) : base(self, stateMachine, "Shoot")
    {
        this.bulletSpawn = bulletSpawn;
        this.aiManager = aiManager;
    }

    public override void Init()
    {
        animator = self.GetComponent<Animator>();
        agent = self.GetComponent<NavMeshAgent>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        timeBetweenShots = 0.1f;
        currentTimerBetweenShots = timeBetweenShots;
    }

    public override void OnBehaviourStart()
    {
        isActive = true;
        animator.SetBool("Idle", true);

        numberOfShots = Random.Range(1, 5);

        Debug.Log("Started " + GetName());
    }

    public override void OnBehaviourEnd()
    {
        isActive = false;
        animator.SetBool("Idle", false);

        numberOfShots = 0;

        Debug.Log("Ended " + GetName());
    }

    public override void OnUpdate()
    {
        if (isActive)
        {
            currentTimerBetweenShots += Time.deltaTime;

            if(currentTimerBetweenShots <= timeBetweenShots && numberOfShots > 0)
            {
                Debug.Log("I shot!  Number of Shots Left: " + numberOfShots);

                RaycastHit hit;
                if (Physics.Raycast(bulletSpawn.transform.position, bulletSpawn.transform.forward, out hit))
                {
                    if (hit.transform.GetComponent<IDamage>() != null)
                    {
                        Debug.Log("I hit the Player!");
                        hit.transform.GetComponent<IDamage>().TakeDamage();
                        aiManager.HitPlayer();
                    }
                }

                currentTimerBetweenShots = 0;
                numberOfShots--;
            }

            if (numberOfShots <= 0)
            {
                Debug.Log("!Entered!");
                stateMachine.HandleEvent(Fabio_AIEvents.FinishedShooting);
            }
        }
    }

    public int GetNumberOfShots()
    {
        return numberOfShots;
    }
}