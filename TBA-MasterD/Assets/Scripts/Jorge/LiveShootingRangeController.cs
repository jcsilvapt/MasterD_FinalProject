using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveShootingRangeController : MonoBehaviour {

    [SerializeField] LiveShootingTarget[] targets;
    [SerializeField] GameObject[] lightsObjects;
    [SerializeField] DoorController mainDoor;
    [SerializeField] DoorController glassDoor;

    private LiveShootingTarget currentTarget;
    private List<Light> lights = new List<Light>();

    private bool isPlayer = false;
    private int targetsCount = 0;


    private void Start() {

        // Add all the lights to the list
        foreach(GameObject b in lightsObjects) {
            lights.Add(b.GetComponentInChildren<Light>());
        }

        // Turn off all the lights at the beginning
        ToggleLights(false);
    }

    private void EnableTarget() {
        if(targetsCount <= targets.Length) {
            targets[targetsCount].TargetRise();
        }
    }

    /// <summary>
    /// Function that turns on the light or off.
    /// </summary>
    /// <param name="value">if the lights are to be on or not</param>
    private void ToggleLights(bool value) {
        foreach (Light b in lights) {
            b.enabled = value;
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.transform.name == "Player" || other.transform.tag == "Player") {
            isPlayer = true;
            mainDoor.CloseDoor();
            mainDoor.LockMode(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (isPlayer) {
            ToggleLights(true);
        }
    }
}
