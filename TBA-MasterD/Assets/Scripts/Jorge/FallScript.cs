using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallScript : MonoBehaviour {

    [SerializeField] Transform[] respawnPositions;

    [SerializeField] Transform currentLastPosition;

    private Transform player;

    private void Start() {
        foreach(Transform b in respawnPositions) {
            b.gameObject.AddComponent<RegisterLastPosition>();
            b.GetComponent<RegisterLastPosition>().RegisterParentScript(this);
        }
    }

    public void SetLastPosition(Transform position) {
        this.currentLastPosition = position;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            player = GameObject.Find("Player").transform;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.position = currentLastPosition.position;

        }
    }

}
