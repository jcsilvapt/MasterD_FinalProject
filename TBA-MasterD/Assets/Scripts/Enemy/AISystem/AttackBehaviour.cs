using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AttackBehaviour : AIBehaviour
{

    //components
    [SerializeField] Transform target;
    [SerializeField] NavMeshAgent navAgent;
    [SerializeField] bool isActive;
    [SerializeField] Enemy enemy;
    private Animator anim;

    //bullet and spawn
    [SerializeField] Object bullet;
    [SerializeField] Transform bulletSpawn;
    [SerializeField] Transform enemyHead;
    [SerializeField] float distanceToShoot;

    public AttackBehaviour(MonoBehaviour self, AIStateMachine stateMachine, Transform selfHead, float distanceToShoot) : base(self, stateMachine, "Attack")
    {
        enemyHead = selfHead;
        this.distanceToShoot = distanceToShoot;
    }

    public override void Init()
    {
        target = GameObject.FindGameObjectWithTag("PlayerParent").transform;
        navAgent = self.GetComponent<NavMeshAgent>();
        enemy = self.GetComponent<Enemy>();
        anim = self.GetComponent<Animator>();

    }
    public override void OnBehaviourEnd()
    {
        isActive = false;
        anim.SetBool("iShoot", false);
    }

    public override void OnBehaviourStart()
    {
        isActive = true;
        enemy.hasAlertedOther = true;
        enemy.Shoot();
        anim.SetBool("iShoot", true);
        //self.GetComponent<Enemy>().CheckSurroundingEnemies();
    }

    public override void OnUpdate()
    {

        if (isActive)
        {
            if (AIUtils_Fabio.HasVisionOfPlayer(enemyHead.transform, target, self.GetComponent<Enemy>().GetDistanceToView())) //checks if player is in view
            {
                Vector3 distance = target.transform.position - self.transform.position;

                if (distance.magnitude <= distanceToShoot)  // checks if the distance of the player is enough to shoot
                {
                    self.transform.LookAt(target.position);
                    enemy.Shoot();
                    enemy.SetShooting(true);

                }
                else
                {
                    stateMachine.HandleEvent(AIEvents.RangeToFar);
                    enemy.SetShooting(false);
                    return;
                }
            }
            else
            {
                stateMachine.HandleEvent(AIEvents.LostPlayer);
                enemy.SetShooting(false);
                return;
            }
        }
    }



}
