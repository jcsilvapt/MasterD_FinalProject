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
    }

    public override void OnBehaviourStart()
    {
        isActive = true;
        animator.SetBool("IsShooting", true);

        numberOfShots = Random.Range(1, 5);
        currentTimerBetweenShots = timeBetweenShots;

        Debug.Log("Started " + GetName());
    }

    public override void OnBehaviourEnd()
    {
        isActive = false;
        animator.SetBool("IsShooting", false);

        numberOfShots = 0;
        currentTimerBetweenShots = 0;

        Debug.Log("Ended " + GetName());
    }

    public override void OnUpdate()
    {
        if (isActive)
        {
            currentTimerBetweenShots += Time.deltaTime;

            if(currentTimerBetweenShots >= timeBetweenShots && numberOfShots > 0)
            {
                Debug.Log("I shot!  Number of Shots Left: " + numberOfShots);

                RaycastHit hit;
                if (Physics.Raycast(bulletSpawn.transform.position, bulletSpawn.transform.forward, out hit))
                {
                    if (hit.transform.GetComponent<IDamage>() != null)
                    {
                        hit.transform.GetComponent<IDamage>().TakeDamage();
                        
                        if(hit.transform.gameObject.tag == "Player")
                        {
                            numberOfShots--;

                            Debug.Log("I hit the Player!");
                            Debug.Log("Shoot! I still have " + numberOfShots + " shots left!");
                            aiManager.HitPlayer();

                            return;
                        }
                    }
                }

                currentTimerBetweenShots = 0;
                numberOfShots--;
            }

            if (numberOfShots <= 0)
            {
                Debug.Log("I don't have any more bullets!");
                stateMachine.HandleEvent(Fabio_AIEvents.FinishedShooting);
            }
        }
    }

    public int GetNumberOfShots()
    {
        return numberOfShots;
    }
}