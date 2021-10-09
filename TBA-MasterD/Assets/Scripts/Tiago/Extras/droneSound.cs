using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class droneSound : MonoBehaviour
{
    private bool hasPassed = false;
    [SerializeField] AudioSource keyboard;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Drone")
        {
            if (!hasPassed)
            {
                keyboard.Play();
                hasPassed = true;
            }
        }
    }
}
