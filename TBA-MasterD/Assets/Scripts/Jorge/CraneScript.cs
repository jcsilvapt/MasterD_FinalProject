using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneScript : MonoBehaviour {

    public Transform player = null;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            player = GameObject.Find("Player").transform;
            player.parent = transform;
        }
    }


    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            player.parent = null;
            player = null;
        }
    }
}
