using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {

    public static SceneController ins;

    [SerializeField] List<GameObject> gameObjects;


    private void Awake() {
        if(ins == null) {
            ins = this;
        }
    }

    public static void AsyncEnable() {
        if(ins != null) {
            ins.StartCoroutine(ins.EnableAllObjectsAsync());
        }
    }


    IEnumerator EnableAllObjectsAsync() {

        // Loads All GameObjects
        foreach(GameObject b in gameObjects) {
            b.SetActive(true);
            yield return new WaitForSeconds(1f);
        }

    }

}
