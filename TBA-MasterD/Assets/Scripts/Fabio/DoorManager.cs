using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour, IDamage
{
    //Doors To Be Locked
    [SerializeField] private DoorController[] toBeLocked;

    //Doors To Be Unlocked
    [SerializeField] private DoorController[] toBeUnlocked;

    //Doors To Be Closed
    [SerializeField] private DoorController[] toBeClosed;

    public void TakeDamage()
    {
        DoorInteraction();
    }

    public void DoorInteraction()
    {
        if (toBeLocked.Length > 0)
        {
            foreach (DoorController door in toBeLocked)
            {
                door.LockMode(true);
            }
        }

        if (toBeUnlocked.Length > 0)
        {
            foreach (DoorController door in toBeUnlocked)
            {
                door.LockMode(false);
            }
        }

        if (toBeClosed.Length > 0)
        {
            foreach (DoorController door in toBeClosed)
            {
                door.CloseDoor();
            }
        }
    }
}
