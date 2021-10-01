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
    private AudioSource shootSoundSource;
    private GameObject bulletParticleSystem;

    #endregion

    private int numberOfShots;

    private float timeBetweenShots;
    private float currentTimerBetweenShots;

    public Fabio_ShootBehaviour(MonoBehaviour self, Fabio_AIStateMachine stateMachine, Transform bulletSpawn, Fabio_AIManager aiManager, AudioSource shootSoundSource, GameObject bulletParticleSystem) : base(self, stateMachine, "Shoot")
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

        target = GameObject.FindGameObjectWithTag("Player").transform;

        timeBetweenShots = 0.1f;
    }

    public override void OnBehaviourStart()
    {
        isActive = true;
        animator.SetBool("IsShooting", true);

        numberOfShots = Random.Range(1, 5);
        currentTimerBetweenShots = timeBetweenShots;
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
        {
            currentTimerBetweenShots += Time.deltaTime;

            if(currentTimerBetweenShots >= timeBetweenShots && numberOfShots > 0)
            {
                bulletParticleSystem.SetActive(true);
                ShootSound();

                Vector3 spray = SetSpray();

                RaycastHit hit;
                if (Physics.Raycast(bulletSpawn.transform.position, (target.transform.position + Vector3.up) - bulletSpawn.position + spray, out hit))
                {
                    if (hit.transform.GetComponent<IDamage>() != null)
                    {
                        hit.transform.GetComponent<IDamage>().TakeDamage();
                        
                        if(hit.transform.gameObject.tag == "Player")
                        {
                            numberOfShots--;

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
                bulletParticleSystem.SetActive(false);
                stateMachine.HandleEvent(Fabio_AIEvents.FinishedShooting);
            }
        }
    }

    public int GetNumberOfShots()
    {
        return numberOfShots;
    }

    private Vector3 SetSpray()
    {
        Vector3 spray = new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f));
        
        return spray;
    }

    private void ShootSound() // used for shooting sounds in an event on the animation
    {
        shootSoundSource.PlayOneShot(shootSoundSource.clip);
    }
}