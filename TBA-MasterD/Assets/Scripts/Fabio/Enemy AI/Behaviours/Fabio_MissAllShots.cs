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
    }

    public override void OnBehaviourStart()
    {
        isActive = true;
        animator.SetBool("IsShooting", true);

        numberOfShots = 0;
        currentTimerBetweenShots = 0;

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

            if (currentTimerBetweenShots >= timeBetweenShots && numberOfShots > 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(bulletSpawn.transform.position, GetMissedShot(), out hit))
                {
                    if (hit.transform.GetComponent<IDamage>() != null)
                    {
                        hit.transform.GetComponent<IDamage>().TakeDamage();
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
        Debug.Log("Set All Shots was called! It has " + shots + " shots left!");
        numberOfShots = shots;
    }

    public Vector3 GetMissedShot()
    {
        Vector3 bulletDirection = bulletSpawn.transform.forward;

        int isXPositive = Random.Range(0, 2);
        int isYPositive = Random.Range(0, 2);

        float xMissedDirection = Random.Range(3f, 12f);
        float yMissedDirection = Random.Range(3f, 12f);

        xMissedDirection = (isXPositive == 0) ? xMissedDirection : -xMissedDirection;
        yMissedDirection = (isYPositive == 0) ? yMissedDirection : -yMissedDirection;

        return bulletDirection = new Vector3(bulletDirection.x + xMissedDirection, bulletDirection.y + yMissedDirection, bulletDirection.z);
    }
}