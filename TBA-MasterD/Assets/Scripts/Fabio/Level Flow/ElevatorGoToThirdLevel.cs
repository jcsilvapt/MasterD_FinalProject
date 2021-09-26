using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorGoToThirdLevel : MonoBehaviour
{
    //Elevator Controller Reference
    [SerializeField] private ElevatorController elevator;

    //Resources Replenish Level Flow Reference
    [SerializeField] private ResourcesRoomReplenishResources resourcesReplenish;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
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
