using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterLastPosition : MonoBehaviour {

    private FallScript parentScript;

    public void RegisterParentScript(FallScript script) {
        this.parentScript = script;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            parentScript.SetLastPosition(transform);
        }   
    }

}
