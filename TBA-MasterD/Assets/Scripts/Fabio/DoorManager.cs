using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour, IDamage {
    [Tooltip("Doors To Be Locked")]
    [SerializeField] private DoorController[] toBeLocked;

    [Tooltip("Doors To Be Unlocked")]
    [SerializeField] private DoorController[] toBeUnlocked;

    [Tooltip("Doors To Be Closed")]
    [SerializeField] private DoorController[] toBeClosed;

    [Tooltip("Detect if Player's Can Interact using Inputs")]
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
        if (!alreadyInteracted) {
            DoorInteraction();

            if (hasAIManagerAssociated) {
                aiManager.PlayerDetected();
            }

            alreadyInteracted = true;
            canInteract = false;
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
