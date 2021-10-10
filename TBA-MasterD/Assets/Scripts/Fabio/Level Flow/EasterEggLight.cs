using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggLight : MonoBehaviour
{
    [SerializeField] private SecondLevelManager levelManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<MeshRenderer>().enabled = false;
            levelManager.SetEasterEggAudio();
        }
    }
}
