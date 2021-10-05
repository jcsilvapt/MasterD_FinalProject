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
    private AudioSource shootSoundSource;
    private GameObject bulletParticleSystem;

    #endregion

    private int numberOfShots;

    private float timeBetweenShots;
    private float currentTimerBetweenShots;


    public Fabio_MissAllShots(MonoBehaviour self, Fabio_AIStateMachine stateMachine, Transform bulletSpawn, int numberOfShots,Fabio_AIManager aiManager, AudioSource shootSoundSource, GameObject bulletParticleSystem) : base(self, stateMachine, "MissAllShots")
    {
        this.bulletSpawn = bulletSpawn;
        this.aiManager = aiManager;
        this.shootSoundSource = shootSoundSource;
        this.bulletParticleSystem = bulletParticleSystem;
    }

    public override void Init()
    {
        animator = self.GetComponent<Animator>();
        agent = self.GetComponent<NavMeshAgent>();

        target = GameObject.FindGameObjectWithTag("PlayerParent").transform;

        timeBetweenShots = 0.1f;
    }

    public override void OnBehaviourStart()
    {
        isActive = true;
        animator.SetBool("IsShooting", true);

        numberOfShots = 0;
        currentTimerBetweenShots = 0;
    }

    public override void OnBehaviourEnd()
    {
        isActive = false;
        animator.SetBool("IsShooting", false);

        numberOfShots = 0;
        currentTimerBetweenShots = 0;
    }

    public override void OnUpdate()
    {
        if (isActive)
        {   currentTimerBetweenShots += Time.deltaTime;

            if (currentTimerBetweenShots >= timeBetweenShots && numberOfShots > 0)
            {
                bulletParticleSystem.SetActive(true);
                ShootSound();

                Vector3 missed = GetMissedShot();
                Debug.DrawRay(bulletSpawn.transform.position, missed, Color.blue, 5f);
                RaycastHit hit;
                if (Physics.Raycast(bulletSpawn.transform.position, missed, out hit))
                {
                    if (hit.transform.GetComponent<IDamage>() != null)
                    {
                        if(!hit.transform.gameObject == self)
                        {
                            hit.transform.GetComponent<IDamage>().TakeDamage();
                        }
                    }
                }

                currentTimerBetweenShots = 0;
                numberOfShots--;
            }

            if (numberOfShots <= 0)
            {
                bulletParticleSystem.SetActive(false);
                stateMachine.HandleEvent(Fabio_AIEvents.FinishedShooting);
            }
        }
    }

    public void SetAllShots(int shots)
    {
        numberOfShots = shots;
    }

    public Vector3 GetMissedShot()
    {
        Vector3 bulletDirection = (target.transform.position + Vector3.up) - bulletSpawn.position;

        int isXPositive = Random.Range(0, 2);
        int isYPositive = Random.Range(0, 2);

        float yMissedDirection = Random.Range(1.5f, 4f);
        float xMissedDirection = Random.Range(1.5f, 4f);
        float zMissedDirection = Random.Range(1.5f, 4f);

        xMissedDirection = (isXPositive == 0) ? xMissedDirection : -xMissedDirection;
        yMissedDirection = (isYPositive == 0) ? yMissedDirection : -yMissedDirection;

        bulletDirection += new Vector3(xMissedDirection, yMissedDirection, zMissedDirection);

        return bulletDirection;
    }

    private void ShootSound() // used for shooting sounds in an event on the animation
    {
        shootSoundSource.PlayOneShot(shootSoundSource.clip);
    }
}