using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialLevelManager : MonoBehaviour {

    public static TutorialLevelManager ins;

    [SerializeField] List<GameObject> gameObjects;

    #region EVENTS
    public event Action triggerAction;
    #endregion

    private void Awake() {
        ins = this;
    }

    public void TriggerAction() {
        if(triggerAction != null) {
            triggerAction();
            
        }
    }

    public static void AsyncDisable() {
        if(ins != null) {
            ins.DisableStuff();
        }
    }

    private void DisableStuff() {
        foreach(GameObject b in gameObjects) {
            b.SetActive(false);
        }
    }

 //   IEnumerator Disa


}
