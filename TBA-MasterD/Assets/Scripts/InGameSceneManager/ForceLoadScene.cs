using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForceLoadScene : MonoBehaviour {

    [Header("Scene Controller Load Next Scene")]
    [SerializeField] string sceneToLoad;

    private void Start() {
        Debug.LogWarning("HERE:::::::::::::::::::::::::");
        Debug.Log("ForceLoadScene: Loading Next Scene");
        GameManager.ChangeScene(sceneToLoad, false, true);
    }

}
