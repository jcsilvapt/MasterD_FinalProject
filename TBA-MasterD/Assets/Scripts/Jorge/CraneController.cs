using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneController : MonoBehaviour {

    [Tooltip("Add Here all the moving parts objects in the scene")]
    [SerializeField] GameObject[] movingParts;

    [SerializeField] bool isActive = false;

    [SerializeField] KeyCode keyToInteract;


    private void Start() {
        if(!isActive) {
            ShutDownCrane();
        } else {
            ActivateCrane();
        }
    }


    private void ActivateCrane() {
        foreach(GameObject g in movingParts) {
            Animator b = g.GetComponent<Animator>();
            b.enabled = true;
        }
        isActive = !isActive;
    }    
    
    private void ShutDownCrane() {
        foreach(GameObject g in movingParts) {
            Animator b = g.GetComponent<Animator>();
            b.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "Player") {
            if(Input.GetKeyDown(keyToInteract)) {
                if (!isActive) {
                    ActivateCrane();
                }
            }
        }
    }

}
