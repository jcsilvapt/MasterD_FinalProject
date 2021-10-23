using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoReplenishBullets : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            charController player = other.transform.parent.parent.GetComponent<charController>();
            player.ReplenishBullets();
        }
    }

}
