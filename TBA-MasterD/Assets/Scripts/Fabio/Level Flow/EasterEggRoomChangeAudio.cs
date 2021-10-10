using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggRoomChangeAudio : MonoBehaviour
{
    [SerializeField] private SecondLevelManager levelManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelManager.SetAudioToPlay();
        }
    }
}
