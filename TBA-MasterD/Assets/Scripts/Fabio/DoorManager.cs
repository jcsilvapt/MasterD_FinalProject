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

    //Detect if Player's Can Interact
    [SerializeField] private bool canInteract;

    //Detect if Player already interacted
    private bool alreadyInteracted;

    [Header("Second Level Related")]
    [SerializeField] private bool hasAIManagerAssociated;
    [SerializeField] private Fabio_AIManager aiManager;

    private void Start()
    {
        canInteract = false;
        alreadyInteracted = false;
    }

    private void Update()
    {
        if(canInteract && !alreadyInteracted && Input.GetKeyDown(KeyMapper.inputKey.Interaction))
        {
            TakeDamage();
            canInteract = false;
            alreadyInteracted = true;
            this.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
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

    public void TakeDamage()
    {
        DoorInteraction();

        if (hasAIManagerAssociated)
        {
            aiManager.PlayerDetected();
        }
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
