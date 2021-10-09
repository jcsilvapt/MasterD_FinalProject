using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : MonoBehaviour {

    [SerializeField] GameObject Item;

    [SerializeField] Modal modal;

    [SerializeField] charController player = null;

    private bool isActive = false;
    private bool isUsed = false;

    private void Update() {
        if(isActive && !isUsed && player != null) {
            if(Input.GetKeyDown(KeyMapper.inputKey.Interaction)) {
                player.SetDroneControl(true);
                player.EnableInteractionUI(false);
                Item.SetActive(false);
                isUsed = true;
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (!isUsed && !isActive) {
            if (other.CompareTag("Player") || other.CompareTag("PlayerParent")) {
                player = other.transform.parent.parent.GetComponent<charController>();
                player.EnableInteractionUI(true);
                isActive = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(isActive && !isUsed) {
            if (other.CompareTag("Player") || other.CompareTag("PlayerParent")) {
                player.EnableInteractionUI(false);
                player = null;
                isActive = false;
            }
        }
    }

}
