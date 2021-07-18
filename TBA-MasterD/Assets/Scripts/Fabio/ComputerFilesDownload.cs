using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerFilesDownload : MonoBehaviour
{
    private bool playerInside;

    private void Update()
    {
        if (!playerInside)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SecondLevelManager.instance.StartEnemyWaves();
            Destroy(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInside = false;
        }
    }
}
