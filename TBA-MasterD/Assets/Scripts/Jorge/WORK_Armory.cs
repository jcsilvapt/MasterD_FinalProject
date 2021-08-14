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
                if(Input.GetKeyDown(KeyMapper.inputKey.Interaction)) {
                    GameObject.Find("Player").GetComponent<charController>().EnableWeapon();
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
