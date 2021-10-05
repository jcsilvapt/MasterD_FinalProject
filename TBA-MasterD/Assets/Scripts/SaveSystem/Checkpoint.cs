using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour {

    [Header("Setup")]
    [Tooltip("Input the scene Index")]
    [SerializeField] int sceneIndex;
    [Tooltip("Add here all the enemies (objects) represented in the scene")]
    [SerializeField] List<GameObject> enemiesInScene;
    [Tooltip("Set true if you wish to make this checkpoint recurrent")]
    [SerializeField] bool isMultiple = false;

    [Header("Developer Settings")]
    [Tooltip("Display the status of the current checkpoint, is ignored if isMultiple is true")]
    [SerializeField] bool isActivated = false;

    private void OnTriggerEnter(Collider other) {
        if(isMultiple) {
            // TODO
        } else {
            if(!isActivated) {
                if (other.CompareTag("Player")) {
                    if(enemiesInScene.Count > 0) { // Confirms if the scene has enemies and perform accordingly
                        SaveSystemManager.Save(this, enemiesInScene);
                    } else {
                        SaveSystemManager.Save(this);
                    }
                }
                isActivated = true;
            }
        }
        other.transform.parent.parent.GetComponent<charController>().lastCheckpoint = this;
    }

    public void LoadLastCheckPoint() {
        SaveSystemManager.Load();
    }

    public List<GameObject> GetEnemiesInScene() {
        return enemiesInScene;
    }

    public void SetEnemiesStats(List<GameObject> enemies) {
        enemiesInScene = enemies;
    }

    public int GetCurrentSceneIndex() {
        return sceneIndex;
    }
}
