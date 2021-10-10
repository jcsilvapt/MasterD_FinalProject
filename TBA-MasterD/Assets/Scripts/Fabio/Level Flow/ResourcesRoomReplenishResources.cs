using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesRoomReplenishResources : MonoBehaviour
{
    //Player Reference
    private charController player;

    //Detect if Player already interacted
    private bool alreadyInteracted;

    //Detect if Player can Interact using Inputs
    private bool canInteract;

    //Flag Controlling if UI is shown to the player
    private bool showUI;

    private void Start()
    {

    }

    private void Update()
    {
        if (alreadyInteracted)
        {
            return;
        }

        if (!alreadyInteracted && player != null)
        {
            if (showUI)
            {
                player.EnableInteractionUI(true);
            }
        }

        if (canInteract && !alreadyInteracted && Input.GetKeyDown(KeyMapper.inputKey.Interaction))
        {
            player.ReplenishHealth();
            player.ReplenishBullets();
            player.EnableInteractionUI(false);
            showUI = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player = other.transform.parent.parent.GetComponent<charController>();
            canInteract = true;
            showUI = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if(player != null)
            {
                player.EnableInteractionUI(false);
            }
            player = null;
            canInteract = false;
            showUI = false;
        }
    }
}
