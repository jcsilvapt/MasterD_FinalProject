using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : MonoBehaviour {

    [SerializeField] GameObject Item;


    private void OnTriggerStay(Collider other) {
        if(other.tag == "Player") {
            if(Input.GetKeyDown(KeyCode.E)) {
                FindObjectOfType<charController>().SetDroneControl(true);
                Item.SetActive(false);
            }
        }
    }

}
