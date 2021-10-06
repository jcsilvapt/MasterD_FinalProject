using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
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

    //Flag Controlling if UI is shown to the player
    private bool showUI;

    [Tooltip("If true, can only work by interaction and not by shooting at it")]
    [SerializeField] private bool isOnlyManuallyInteractive;

    private bool canManuallyInteract;
    private bool isInteracting;

    [Header("Related To Tutorial Map")]
    [SerializeField] CraneController cc;


    [Header("Second Level Related")]
    [SerializeField] private bool hasAIManagerAssociated;
    [SerializeField] private Fabio_AIManager aiManager;

    [Header("Developer Settings")]
    private charController player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerParent").GetComponent<charController>();

        canInteract = false;
        alreadyInteracted = false;
        showUI = false;
    }

    private void Update()
    {
        if (!alreadyInteracted && player != null)
        {
            if (showUI)
            {
                player.EnableInteractionUI(true);
            }
        }
        
        if(canInteract && !alreadyInteracted && Input.GetKeyDown(KeyMapper.inputKey.Interaction))
        {
            isInteracting = true;
            TakeDamage();
            isInteracting = false;
            showUI = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player = other.transform.parent.parent.GetComponent<charController>();
            canInteract = true;
            canManuallyInteract = true;
            showUI = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.EnableInteractionUI(false);
            player = null;
            canInteract = false;
            canManuallyInteract = false;
            showUI = false;
        }
    }

    public void TakeDamage()
    {
        if (!alreadyInteracted) {
            if (!isOnlyManuallyInteractive)
            {
                DoorInteraction();
                alreadyInteracted = true;
            }
            else
            {
                if (canManuallyInteract)
                {
                    if (isInteracting)
                    {
                        DoorInteraction();
                        alreadyInteracted = true;
                    }
                }
            }

            if (hasAIManagerAssociated) {
                aiManager.PlayerDetected();
            }

            if(cc != null) {
                cc.TakeDamage();
            }

            if(player != null)
            {
                player.EnableInteractionUI(false);
            }
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
