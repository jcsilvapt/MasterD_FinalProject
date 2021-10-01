using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMachineElevatorControlRoomCallElevator : MonoBehaviour
{
    #region References

    //Elevator Controller Reference
    [SerializeField] private ElevatorController elevator;

    #endregion

    #region Control Variables

    private bool canInteract;
    private bool alreadyInteracted;

    #endregion


    private void Start()
    {
        canInteract = false;
        alreadyInteracted = false;
    }

    private void Update()
    {
        if (canInteract && !alreadyInteracted)
        {
            UIManager.UI_ToggleCrosshair(false);
        }
        else
        {
            UIManager.UI_ToggleCrosshair(true);
        }

        if (canInteract && !alreadyInteracted && Input.GetKeyDown(KeyMapper.inputKey.Interaction))
        {
            elevator.SetAvailability(true);
            canInteract = false;
            alreadyInteracted = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canInteract = false;
        }
    }

}
