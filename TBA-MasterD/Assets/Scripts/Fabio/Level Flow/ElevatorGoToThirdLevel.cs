using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorGoToThirdLevel : MonoBehaviour
{
    #region References

    //Elevator Controller Reference
    [SerializeField] private ElevatorController elevator;

    //Resources Replenish Level Flow Reference
    [SerializeField] private ResourcesRoomReplenishResources resourcesReplenish;

    //Array of AI Manager References
    [SerializeField] private Fabio_AIManager[] aiManagers;

    #endregion

    #region Control Variables

    private bool canClose;

    private int howManyAIManagersAreWorking;

    #endregion

    private void Update()
    {
        howManyAIManagersAreWorking = 0;

        foreach (Fabio_AIManager manager in aiManagers)
        {
            if (manager.GetIsAIManagerWorking())
            {
                Debug.LogError("Still working: " + manager.name);
                howManyAIManagersAreWorking++;
                break;
            }
        }

        if(howManyAIManagersAreWorking > 0)
        {
            canClose = false;
        }
        else
        {
            canClose = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (canClose)
        {
            if (other.tag == "Player")
            {
                //Stop Player Movement
                elevator.CloseElevatorDoor();
                //Load Next Level

                //Destroy Resources Replenish
                resourcesReplenish.TurnOff();
                resourcesReplenish.gameObject.SetActive(false);
            }
        }
    }
}
