using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialLevelManager : MonoBehaviour {

    public static TutorialLevelManager ins;

    [SerializeField] Animator[] targets;

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
