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
    private void UpdateSceneObjects(SceneSwitcher sceneSwitcher, List<GameObject> gameObjectList) {

        foreach (GameObject objecto in Object.FindObjectsOfType(typeof(GameObject))) {
            if (objecto.CompareTag("Elevator")) {
                sceneSwitcher.SetObjectToMove(objecto.GetComponent<Transform>());
            }
        }

        if (sceneSwitcher.GetTypeOfSwitch()) {
            StartCoroutine(DisableAllObjectsAsync(gameObjectList, sceneSwitcher));
        } else {
            StartCoroutine(EnableAllObjectsAsync(gameObjectList, sceneSwitcher));
        }
    }

    /// <summary>
    /// Method that loads the object of a scene
    /// </summary>
    /// <param name="SceneName">Scene name to load objects</param>
    public static void AsyncEnable(SceneSwitcher sceneSwitcher, List<GameObject> gameObjectList) {
        if(ins != null) {
            ins.UpdateSceneObjects(sceneSwitcher, gameObjectList);
        }
    }

    /// <summary>
    /// Coroutine that enables all objects with 1 second delay
    /// </summary>
    /// <param name="objectsToActivate"></param>
    /// <returns></returns>
    IEnumerator EnableAllObjectsAsync(List<GameObject> objectsToActivate, SceneSwitcher s) {

        // Loads All GameObjects
        if (objectsToActivate.Count > 0) {
            foreach (GameObject b in objectsToActivate) {
                b.SetActive(true);
                yield return new WaitForSeconds(0.5f);
            }
        }
        s.ToogleTypeOfSwitch();
        s.ToogleExecute();
        if(s.GetIsToMoveObject()) {
            s.SetObjectToPosition();
        }
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
