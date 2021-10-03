using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenElevatorDoor : MonoBehaviour
{
    public void OpenSecondLevelElevatorDoor()
    {
        GameObject.FindGameObjectWithTag("Elevator").GetComponent<ElevatorController>().SetAvailability(true);
    }
}
