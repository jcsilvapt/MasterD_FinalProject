using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveShootingRangeController : MonoBehaviour {

    [SerializeField] LiveShootingTarget[] targets;
    [SerializeField] GameObject[] lightsObjects;
    [SerializeField] DoorController mainDoor;
    [SerializeField] DoorController glassDoor;

    [SerializeField] Color initialColor = Color.red;
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color currentColor;


    public LiveShootingTarget currentTarget;
    private List<Light> lights = new List<Light>();

    private bool isPlayer = false;
    private int targetsCount = 0;

    public bool liveShootingActive = false;

    private bool roomCompleted = false;

    public float intensitivitySpeed = 0.5f;
    private bool enableLights;
    private bool swapLightColor;
    private float currentIntensitivity = 0.0f;

    private void Start() {

        // Disables the final glass door
        glassDoor.LockMode(true);

        // Add all the lights to the list
        foreach (GameObject b in lightsObjects) {
            lights.Add(b.GetComponentInChildren<Light>());
        }

        // Turn off all the lights at the beginning
        ToggleLights(false);

        currentColor = initialColor;
    }

    private bool HasNextTarget() {
        if (targetsCount < targets.Length) {
            return true;
        }
        return false;
    }

    private void SetNextTarget() {
        currentTarget = targets[targetsCount];
        currentTarget.TargetRise();
        targetsCount++;
    }

    private void Update() {
        if (enableLights) {
            ToggleLights(true);
        }
        if(swapLightColor) {
            ChangeLightsColor();
        }
        if (liveShootingActive) {
            if (currentTarget != null) {
                if (currentTarget.HasBeenHit()) {
                    if (!HasNextTarget()) {
                        currentTarget = null;
                    } else {
                        SetNextTarget();
                    }
                }
            } else {
                EnableRoom();
            }
        } else {
            // TODO: Melhorar este sistema...
        }
    }

    /// <summary>
    /// Function that Opens the glass door
    /// </summary>
    private void EnableRoom() {
        liveShootingActive = false;
        swapLightColor = true;
        glassDoor.LockMode(false);
        //mainDoor.LockMode(false);
        roomCompleted = true;
    }


    /// <summary>
    /// Function that turns on the light or off.
    /// </summary>
    /// <param name="value">if the lights are to be on or not</param>
    private void ToggleLights(bool turnOn) {
        foreach (Light b in lights) {
            if (!turnOn) {
                b.intensity = 0;
            } else {
                enableLights = true;
                b.intensity = currentIntensitivity;
                currentIntensitivity += Time.deltaTime * intensitivitySpeed;
            }
        }
        if (currentIntensitivity >= 0.8f) {
            enableLights = false;
        }
    }

    private void ChangeLightsColor() {
        foreach (Light b in lights) {
            b.color = Color.Lerp(b.color, defaultColor, Time.deltaTime * 0.8f);
            if (b.color.Equals(defaultColor))
                swapLightColor = false;
        }
        
    }

    private void OnTriggerStay(Collider other) {
        if (other.transform.name == "Player" || other.transform.tag == "Player" && !roomCompleted) {
            isPlayer = true;
            mainDoor.CloseDoor();
            mainDoor.LockMode(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (isPlayer && !roomCompleted) {
            enableLights = true;
            SetNextTarget();
            liveShootingActive = true;
        }
    }
}
