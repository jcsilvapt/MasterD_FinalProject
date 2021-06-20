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

    //Random Search Points Reference
    private Transform selfRandomSearchPoints;

    //Points where the Enemy Can Search
    private Transform[] randomSearchPoints;

    //Points Previously Chosen
    private Transform[] chosenPoints;

    //Chosen Points Array Index
    private int chosenPointsIndex;

    //Random Point Chosen
    private Transform randomPoint;

    //Flag Indicating if Enemy Choose the Random Point
    private bool randomPointChosen;

    //Flag Indicating if Enemy Reached the Point
    private bool reachedPoint;

    public RandomSearchBehaviour(MonoBehaviour self, AIStateMachine stateMachine) : base(self, stateMachine, "RandomSearch")
    {

    }

    public override void Init()
    {
        //Get Player Reference
        target = GameObject.FindGameObjectWithTag("Player").transform;

        //Get Nav Mesh Agent Component
        agent = self.GetComponent<NavMeshAgent>();

        //Get Self Random Search Points Reference
        selfRandomSearchPoints = self.transform.Find("RandomSearchPoints");

        //Initialize Random Search Points Array
        randomSearchPoints = new Transform[selfRandomSearchPoints.childCount];
        
        //Array Index Auxiliar
        int index = 0;

        //Fill the Array
        foreach(Transform child in selfRandomSearchPoints)
        {
            randomSearchPoints[index] = child;
            index++;
        }

        //Initialize Chosen Points Array
        chosenPoints = new Transform[randomSearchPoints.Length];

        //Set Chosen Points Index to 0
        chosenPointsIndex = 0;

        //Remove Random Search Points from Enemy's Children
        selfRandomSearchPoints.parent = null;
    }

    public override void OnBehaviourEnd()
    {
        Debug.Log("Random Search Behaviour Ended");

        //Set IsActive to False, and Reset all the other Flags.
        isActive = false;
        randomPointChosen = false;
        reachedPoint = false;

        //Clean Chosen Points Array and Reset Index
        chosenPoints = null;
        chosenPointsIndex = 0;

        //Make Self Random Search Points a child of Enemy again and Reset it's position
        selfRandomSearchPoints.parent = self.transform;
        selfRandomSearchPoints.localPosition = Vector3.zero;
    }

    public override void OnBehaviourStart()
    {
        Debug.Log("Random Search Behaviour Started");

        //Set IsActive to True, Set all Flags to False.
        isActive = true;
        randomPointChosen = false;
        reachedPoint = false;

        //Clean Chosen Points Array and Reset Index
        chosenPoints = null;
        chosenPointsIndex = 0;
    }

    public override void OnUpdate()
    {
        //If IsActive is False, return.
        if (!isActive)
        {
            return;
        }

        ChoosePoint();
        SearchPoint();
        ContinueOrEndRandomSearch();
    }

    private void ChoosePoint()
    {
        //If point is chosen, return.
        if (randomPointChosen)
        {
            return;
        }

        //Select a random point
        randomPoint = randomSearchPoints[Random.Range(0, randomSearchPoints.Length - 1)];

        //Verify if Search Point has been Chosen Before.
        //If it was, return.
        foreach(Transform chosenPoint in chosenPoints)
        {
            if(chosenPoint == randomPoint)
            {
                return;
            }
        }

        //If the Path is not Achievable, Add the point to the Chosen Points Array, Increment Chosen Points Index and return
        if (!agent.CalculatePath(randomPoint.position, agent.path))
        {
            chosenPoints[chosenPointsIndex] = randomPoint;
            chosenPointsIndex++;

            return;
        }

        //Set Enemy Destination
        agent.SetDestination(randomPoint.position);

        //Set Flag Random Point Chosen to True
        randomPointChosen = true;
    }

    private void SearchPoint()
    {
        //If Random Point is not yet chosen, return
        if (!randomPointChosen)
        {
            return;
        }

        //If the Enemy reached to point, return
        if (reachedPoint)
        {
            return;
        }

        //If the Enemy reached the Random Point, Set Reached Point Flag to true
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            reachedPoint = true;
        }
    }

    private void ContinueOrEndRandomSearch()
    {
        //If Random Point is not yet chosen, return
        if (!randomPointChosen)
        {
            return;
        }

        //If the Enemy did npt reach the point, return
        if (!reachedPoint)
        {
            return;
        }

        //This will determine if the Enemy Continues or Ends the Random Search
        int continueOrEndVariable = Random.Range(0, 1);

        //IF 0 -> Continue Search (Reset Everything)
        //ELSE 1 -> End Search (Call HandleEvent)
        if(continueOrEndVariable == 0)
        {
            randomPointChosen = false;
            reachedPoint = false;
        }
        else
        {
            //stateMachine.HandleEvent(AIEvents.);
        }
    }
}
