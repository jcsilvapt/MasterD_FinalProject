using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WORK_Armory : MonoBehaviour {

    private bool showHint = false;

    private void OnTriggerStay(Collider other) {
        if(other.tag == "Player") {
            if(!showHint) {
                UIManager.UI_ToggleCrosshair(false);
                showHint = true;
            }

            if(showHint) {
                if(Input.GetKeyDown(KeyCode.E)) {
                    //TODO
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player") {
            if(showHint) {
                UIManager.UI_ToggleCrosshair(true);
                showHint = false;
            }
        }
    }

}
