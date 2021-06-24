using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomSearchBehaviour : AIBehaviour 
{
    //Flag Indicating if this Behaviour is Active
    private bool isActive;

    //Player Reference
    private Transform target;

    //Nav Mesh Agente Reference
    private NavMeshAgent agent;

    //Flag Indicating if Enemy Choose the Random Point
    private bool randomPointChosen;

    //Flag Indicating if Enemy Reached the Point
    private bool reachedPoint;

    //Time Duration for the Enemy to Wait In Point
    private float waitInPointTimeDuration;

    //Timer for Wait In Point
    private float timerWaitInPoint;

    public RandomSearchBehaviour(MonoBehaviour self, AIStateMachine stateMachine) : base(self, stateMachine, "RandomSearch") {

    }

    public override void Init() {
        //Get Player Reference
        target = GameObject.FindGameObjectWithTag("Player").transform;

        //Get Nav Mesh Agent Component
        agent = self.GetComponent<NavMeshAgent>();

        //Set Time Duration for Wait In Point
        waitInPointTimeDuration = 3f;
    }

    public override void OnBehaviourEnd() {
        Debug.Log("Random Search Behaviour Ended");

        //Set IsActive to False, and Reset all the other Flags.
        isActive = false;
        randomPointChosen = false;
        reachedPoint = false;
    }

    public override void OnBehaviourStart() {
        Debug.Log("Random Search Behaviour Started");

        //Set IsActive to True, Set all Flags to False.
        isActive = true;
        randomPointChosen = false;
        reachedPoint = false;
    }

    public override void OnUpdate() {
        //If IsActive is False, return.
        if (!isActive) {
            return;
        }

        //Check if the Enemy sees the Player and act accordingly.
        if (AIUtils_Fabio.HasVisionOfPlayer(self.transform, target, self.GetComponent<Enemy>().GetDistanceToView()))
        {
            stateMachine.HandleEvent(AIEvents.SeePlayer);
            return;
        }

        ChoosePoint();
        SearchPoint();
        WaitInPoint();
    }

    private void ChoosePoint() {

        //If point is chosen, return.
        if (randomPointChosen) {
            return;
        }

        //Create Unit Sphere with Radius 5 (Not sure why Add the Self.transform.position)
        Vector3 randomPoint = Random.insideUnitSphere * 10;
        randomPoint += self.transform.position;

        //Create NavMeshHit, this will take information of the point on the NavMesh
        NavMeshHit navMeshHit;

        //Verify if the Random Point is Valid in the NavMesh
        if(NavMesh.SamplePosition(randomPoint, out navMeshHit, 10, NavMesh.AllAreas))
        {
            agent.SetDestination(navMeshHit.position);

            randomPointChosen = true;
            return;
        }
    }

    private void SearchPoint() {

        //If Random Point is not yet chosen, return
        if (!randomPointChosen) {
            return;
        }

        //If the Enemy reached to point, return
        if (reachedPoint) {
            return;
        }

        //If the Enemy reached the Random Point, Set Reached Point Flag to true and Set Wait In Point Timer
        if (!agent.pathPending && agent.remainingDistance < 0.1f) {
            reachedPoint = true;
            timerWaitInPoint = waitInPointTimeDuration;
        }
    }

    private void WaitInPoint()
    {
        //If Random Point is not yet chosen, return
        if (!randomPointChosen)
        {
            return;
        }

        //If the Enemy reached to point, return
        if (!reachedPoint)
        {
            return;
        }

        if (timerWaitInPoint > 0)
        {
            timerWaitInPoint -= Time.deltaTime;
            return;
        }

        ContinueOrEndRandomSearch();
    }

    private void ContinueOrEndRandomSearch() {

        //If Random Point is not yet chosen, return
        if (!randomPointChosen) {
            return;
        }

        //If the Enemy did npt reach the point, return
        if (!reachedPoint) {
            return;
        }

        //This will determine if the Enemy Continues or Ends the Random Search
        int continueOrEndVariable = Random.Range(0, 2);

        //IF 0 -> Continue Search (Reset Everything)
        //ELSE 1 -> End Search (Call HandleEvent)
        if (continueOrEndVariable == 0) {
            randomPointChosen = false;
            reachedPoint = false;
        } else {
            stateMachine.HandleEvent(AIEvents.ReachedDestination);
        }
    }
}
