using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadNextMapAsync : MonoBehaviour
{
    private bool hasPassed = false;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !hasPassed) {
            hasPassed = true;
            TutorialLevelManager.AsyncDisable();
        }
    }
}
