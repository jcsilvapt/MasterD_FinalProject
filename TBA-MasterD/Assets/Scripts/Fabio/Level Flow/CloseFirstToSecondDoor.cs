using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseFirstToSecondDoor : MonoBehaviour
{
    [SerializeField] private SecondLevelManager secondLevelManager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            secondLevelManager.LockDoorFirstToSecondLevel();
        }

        Destroy(gameObject);
    }
}
