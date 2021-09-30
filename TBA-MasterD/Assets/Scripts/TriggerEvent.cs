using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        TutorialLevelManager.ins.TriggerAction();
    }



}
