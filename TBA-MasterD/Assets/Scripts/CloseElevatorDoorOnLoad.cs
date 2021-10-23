using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseElevatorDoorOnLoad : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player") || other.CompareTag("PlayerParent")) {
            GameObject.FindGameObjectWithTag("SecretDoor").GetComponent<Animator>().SetBool("gotOpen", true); // Fecha a porta ao passar do jogador (ao passaaaarr de um navioooooooo)
        }
    }

}
