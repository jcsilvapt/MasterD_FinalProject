using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialLevelManager : MonoBehaviour {

    public static TutorialLevelManager ins;

    private void Awake() {
        ins = this;
    }

    public event Action onPassShoot;
    public void ResetTargets() {
        if(onPassShoot != null) {
            onPassShoot();
        }
    }


}
