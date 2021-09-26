using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesRoomReplenishResources : MonoBehaviour
{
    //Player Reference
    [SerializeField] private charController player;

    //Control Variable
    private bool isActive;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        if (Input.GetKeyDown(KeyMapper.inputKey.Interaction))
        {
            player.ReplenishHealth();
            player.ReplenishBullets();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isActive = false;
        }
    }

    public void TurnOff()
    {
        isActive = false;
    }
}
