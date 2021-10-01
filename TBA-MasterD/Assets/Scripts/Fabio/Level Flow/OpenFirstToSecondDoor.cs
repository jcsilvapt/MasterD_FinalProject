using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenFirstToSecondDoor : MonoBehaviour
{
    [SerializeField] private SecondLevelManager secondLevelManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            secondLevelManager.LockDoorFirstToSecondLevel();
        }

        Destroy(gameObject);
    }
}
