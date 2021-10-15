using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitcher : MonoBehaviour, ISceneControl {

    [Header("SceneController")]
    [Tooltip("Set the name of the scene to be called")]
    [SerializeField] string sceneName;
    [Header("Settings")]
    [Tooltip("Set all the objects required to be activated/disabled on Load")]
    [SerializeField] List<GameObject> gameObjectList;
    [Tooltip("Disable This if you don't want to reload again this scene")]
    [SerializeField] bool isToReload = true;
    [Tooltip("Set true if you wish to disable the objects")]
    [SerializeField] bool isToDisable = false;

    [Header("Move Object To Scene")]
    [SerializeField] bool isToMoveObject = false;
    [SerializeField] Transform objectToBeMoved;
    [SerializeField] Vector3 posToBeMoved;
    [SerializeField] Vector3 rotToBeMoved;


    [Header("Developer Settings")]
    [SerializeField] bool isToExecute = false;
    [SerializeField] bool isLoaded = false;
    [SerializeField] NewSceneController nController;
    [SerializeField] GameObject player;

    [Header("Second Level Dependencies")]
    [SerializeField] bool hasAIManagerDependency;
    [SerializeField] private Fabio_AIManager[] aiManagers;

    private void Start() {
        //GameManager.ChangeScene(sceneName, false, true);
    }


    /// <summary>
    /// Method that returns all of his Objects to be procecced.
    /// </summary>
    /// <returns>List of Game Objects to be activated.</returns>
    public List<GameObject> GetObjects() {
        return gameObjectList;
    }

    public bool GetTypeOfSwitch() {
        return isToDisable;
    }
    public bool GetIsToExecute() {
        return isToExecute;
    }

    public bool GetIsToReload() {
        return isToReload;
    }

    public bool GetIsToMoveObject() {
        return isToMoveObject;
    }

    public void SetObjectToMove(Transform ObjectTransform) {
        objectToBeMoved = ObjectTransform;
    }

    public Transform GetObjectToMove() {
        return objectToBeMoved;
    }

    public void ToogleTypeOfSwitch() {
        isToDisable = !isToDisable;
        Debug.Log("On the next trigger enter is to Disable Objects? " + isToDisable);
    }
    public void ToogleExecute() {
        isToExecute = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (!isLoaded)
        {
            if (other.CompareTag("Player"))
            {
                if (!hasAIManagerDependency)
                {
                    player = other.gameObject;
                    isToExecute = true;
                    SceneController.AsyncEnable(this, gameObjectList);
                    if (!isToReload) isLoaded = true;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isLoaded)
        {
            if (other.CompareTag("Player"))
            {
                if (hasAIManagerDependency)
                {
                    if (AnyAIManagersActive() > 0)
                    {
                        return;
                    }

                    player = other.gameObject;
                    isToExecute = true;
                    SceneController.AsyncEnable(this, gameObjectList);
                    if (!isToReload) isLoaded = true;
                }
            }
        }
    }

    public void SetObjectToPosition() {
        objectToBeMoved.parent = null;
        objectToBeMoved.position = posToBeMoved;
        objectToBeMoved.rotation = Quaternion.Euler(rotToBeMoved);
    }

    #region Fabio Edit

    public void SetAIManagersArray()
    {
        aiManagers = (Fabio_AIManager[]) FindObjectsOfType(typeof(Fabio_AIManager));
    }

    public int AnyAIManagersActive()
    {
        int howManyAIManagersAreWorking = 0;

        foreach (Fabio_AIManager manager in aiManagers)
        {
            if (manager.GetIsAIManagerWorking())
            {
                howManyAIManagersAreWorking++;
                break;
            }
        }

        return howManyAIManagersAreWorking;
    }

    #endregion
}
