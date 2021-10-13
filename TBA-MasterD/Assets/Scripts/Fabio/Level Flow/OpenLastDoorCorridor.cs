using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLastDoorCorridor : MonoBehaviour
{
    [SerializeField] private DoorController door;

    private bool canActive;

    private void OnTriggerEnter(Collider other)
    {
        if (!canActive)
        {
            if (other.CompareTag("Player"))
            {
                door.LockMode(false);
                canActive = true;
            }
        }
    }
}
