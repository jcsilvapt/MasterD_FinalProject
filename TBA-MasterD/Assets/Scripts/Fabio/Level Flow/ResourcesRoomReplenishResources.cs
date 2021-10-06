using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesRoomReplenishResources : MonoBehaviour
{
    //Player Reference
    private charController player;

    //Control Variable
    private bool isActive;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerParent").GetComponent<charController>();
    }

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
