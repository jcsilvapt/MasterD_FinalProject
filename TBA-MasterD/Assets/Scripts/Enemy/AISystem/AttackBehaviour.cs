using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AttackBehaviour : AIBehaviour {

    public Transform target;
    public NavMeshAgent navAgent;
    public bool isActive;


    //bullet and spawn
    public Object bullet;
    public Transform bulletSpawn;

    public float distanceToShoot = 15;

    public Enemy enemy;


    public AttackBehaviour(MonoBehaviour self, AIStateMachine stateMachine) : base(self, stateMachine, "Attack") {

    }

    public override void Init() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = self.GetComponent<NavMeshAgent>();
        enemy = self.GetComponent<Enemy>();

    }
    public override void OnBehaviourEnd() {
        Debug.Log("Player is gone");
        isActive = false;
    }

    public override void OnBehaviourStart() {
        isActive = true;
        Debug.Log("I'm Attacking the enemy");
        enemy.Shoot();

    }

    public override void OnUpdate() {

        if (isActive) {
            if (AIUtils_Fabio.HasVisionOfPlayer(self.transform, target, self.GetComponent<Enemy>().GetDistanceToView())) //checks if player is in view
            {

                Vector3 distance = target.transform.position - self.transform.position;

                if (distance.magnitude <= distanceToShoot)  // checks if the distance of the player is enough to shoot
                {
                    self.transform.LookAt(target.position);
                    enemy.Shoot();
                } else {
                    Debug.Log("Lost the Player");
                    stateMachine.HandleEvent(AIEvents.RangeToFar);
                    return;
                }
            } else {
                stateMachine.HandleEvent(AIEvents.LostPlayer);
                return;
            }
        }
    }



}
