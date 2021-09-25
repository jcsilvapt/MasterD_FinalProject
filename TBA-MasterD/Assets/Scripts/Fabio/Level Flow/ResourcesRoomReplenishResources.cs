using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesRoomReplenishResources : MonoBehaviour
{
    //Player Reference
    private Weapon[] playerWeapons;

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
            foreach(Weapon weapon in playerWeapons)
            {
                weapon.ReplenishBullets();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isActive = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isActive = false;
        }
    }
}
