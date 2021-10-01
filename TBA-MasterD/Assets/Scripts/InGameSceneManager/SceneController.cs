using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Scene that controls the change of the scene
/// </summary>
public class SceneController : MonoBehaviour {

    public static SceneController ins;

    private void Awake() {
        if(ins == null) {
            ins = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Method that get's a object and activates them with a certain delay.
    /// </summary>
    private void UpdateSceneObjects(string SceneName) {
        List<GameObject> sceneObjects = new List<GameObject>();
        foreach(GameObject obj in Object.FindObjectsOfType(typeof(GameObject))) {
            if(obj.scene.name.CompareTo(SceneName) == 0 && obj.GetComponent<SceneSwitcher>() != null) {
                SceneSwitcher s = obj.GetComponent<SceneSwitcher>();
                if(s.GetTypeOfSwitch()) { // If is to Disable
                    StartCoroutine(DisableAllObjectsAsync(s.GetObjects(), s));
                    return;
                } else { // Is to Enable
                    StartCoroutine(EnableAllObjectsAsync(s.GetObjects(), s));
                    return;
                }

            }
        }
    }

    /// <summary>
    /// Method that loads the object of a scene
    /// </summary>
    /// <param name="SceneName">Scene name to load objects</param>
    public static void AsyncEnable(string SceneName) {
        if(ins != null) {
            ins.UpdateSceneObjects(SceneName);
        }
    }

    /// <summary>
    /// Coroutine that enables all objects with 1 second delay
    /// </summary>
    /// <param name="objectsToActivate"></param>
    /// <returns></returns>
    IEnumerator EnableAllObjectsAsync(List<GameObject> objectsToActivate, SceneSwitcher s) {

        // Loads All GameObjects
        foreach(GameObject b in objectsToActivate) {
            b.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
        s.ToogleTypeOfSwitch();
        s.ToogleExecute();
    }

    /// <summary>
    /// Coroutine that disables all objects asap
    /// </summary>
    /// <param name="objectsToDisable"></param>
    /// <returns></returns>
    IEnumerator DisableAllObjectsAsync(List<GameObject> objectsToDisable, SceneSwitcher s) {

        foreach(GameObject b in objectsToDisable) {
            b.SetActive(false);
            yield return null;
        }
        s.ToogleTypeOfSwitch();
        s.ToogleExecute();
    }

}
