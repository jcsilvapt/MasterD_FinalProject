using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fabio_MissAllShots : Fabio_AIBehaviour
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


    public Fabio_MissAllShots(MonoBehaviour self, Fabio_AIStateMachine stateMachine, Transform bulletSpawn, int numberOfShots,Fabio_AIManager aiManager) : base(self, stateMachine, "MissAllShots")
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

        numberOfShots = 0;

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
        /*
         * 
         *    !!!!! FALTA FALHAR O TIRO !!!!!
         *    
         *    
         *    METER OFFSET ABSURDO NA DIRECÇÃO DA BALA!
         * 
         * 
         * 
         * */
        if (isActive)
        {
            currentTimerBetweenShots += Time.deltaTime;

            if (currentTimerBetweenShots >= timeBetweenShots && numberOfShots > 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(bulletSpawn.transform.position, bulletSpawn.transform.forward, out hit))
                {
                    if (hit.transform.GetComponent<IDamage>() != null)
                    {
                        hit.transform.GetComponent<IDamage>().TakeDamage();
                        aiManager.HitPlayer();
                    }
                }

                currentTimerBetweenShots = 0;
                numberOfShots--;
            }

            if (numberOfShots <= 0)
            {
                stateMachine.HandleEvent(Fabio_AIEvents.FinishedShooting);
            }
        }
    }

    public void SetAllShots(int shots)
    {
        numberOfShots = shots;
    }
}