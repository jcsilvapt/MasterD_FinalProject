using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialLevelManager : MonoBehaviour {

    public static TutorialLevelManager ins;

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
}
