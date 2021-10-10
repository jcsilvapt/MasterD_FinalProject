using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScene : MonoBehaviour {


    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            GameManager.ChangeScene(2, false);
        }
    }


}
