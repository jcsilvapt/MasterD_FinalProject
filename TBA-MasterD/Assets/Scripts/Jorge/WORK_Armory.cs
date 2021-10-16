using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WORK_Armory : MonoBehaviour {

    private bool showHint = false;
    private charController player;

    private bool hasBeenUsed = false;

    private void OnTriggerStay(Collider other) {
        if (!hasBeenUsed) {
            if (other.CompareTag("Player")) {
                player = other.transform.parent.parent.GetComponent<charController>();
                if (!showHint) {
                    player.EnableInteractionUI(true);
                    showHint = true;
                }

                if (showHint) {
                    if (Input.GetKeyDown(KeyMapper.inputKey.Interaction)) {
                        player.EnableWeapon();
                        hasBeenUsed = true;
                        player.EnableInteractionUI(false);
                        showHint = false;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            if (showHint) {
                player.EnableInteractionUI(false);
                player = null;
                showHint = false;
            }
        }
    }

}
