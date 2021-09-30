using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextMapAsync : MonoBehaviour {

    private bool hasPassed = false;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player") && !hasPassed) {
            hasPassed = true;
            SceneController.AsyncEnable();
        }
    }
    /*
    IEnumerator LoadSceneAsyncJ() {
        AsyncOperation op = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        while (!op.isDone) {
            yield return null;
        }
    }
    */
}
