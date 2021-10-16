using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveShootingRangeController : MonoBehaviour {

    [Header("Targets")]
    [SerializeField] LiveShootingTarget[] targets;
    [SerializeField] GameObject[] lightsObjects;
    [SerializeField] DoorController mainDoor;
    [SerializeField] DoorController glassDoor;

    [SerializeField] Color initialColor = Color.red;
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color currentColor;

    [SerializeField] DoorController doorToSecondLevel;

    [Header("Audio Settings")]
    [SerializeField] Subtitles initialSound;
    [SerializeField] GameObject warningSound;
    [SerializeField] GameObject earPieceSound;


    [Header("Developer")]
    public LiveShootingTarget currentTarget;
    private List<Light> lights = new List<Light>();
    [SerializeField] bool playedFirstAudio = false;
    [SerializeField] bool playedSecondAudio = false;

    public bool isPlayer = false;
    public int targetsCount = 0;

    public bool liveShootingActive = false;

    public bool roomCompleted = false;

    public float intensitivitySpeed = 0.5f;
    public bool enableLights;
    public bool swapLightColor;
    public float currentIntensitivity = 0.0f;

    private void Start() {

        // Disables the final glass door
        glassDoor.LockMode(true);

        // Add all the lights to the list
        foreach (GameObject b in lightsObjects) {
            lights.Add(b.GetComponentInChildren<Light>());
        }

        // Turn off all the lights at the beginning
        ToggleLights(false);

        warningSound.SetActive(false);
        earPieceSound.SetActive(false);
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
            if(!mainDoor.IsLocked() && roomCompleted) {
                warningSound.SetActive(true);
                earPieceSound.SetActive(true);
                mainDoor.LockMode(false);
                mainDoor.SetAlwaysOpen();
            }
        }
    }

    /// <summary>
    /// Function that Opens the glass door
    /// </summary>
    private void EnableRoom() {
        liveShootingActive = false;
        swapLightColor = true;
        glassDoor.LockMode(false);
        glassDoor.SetAlwaysOpenSlideDoor();
        //mainDoor.LockMode(false);
        roomCompleted = true;
        try {
        doorToSecondLevel = GameObject.FindGameObjectWithTag("DoorToSecondLevel").GetComponent<DoorController>();   

        } catch {
            Debug.LogError("No Door to second Level");
        }
        if(doorToSecondLevel != null) {
            doorToSecondLevel.LockMode(false);
        }
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
        if (currentIntensitivity >= 0.9f) {
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

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") || other.CompareTag("PlayerParent")) {
            if (!roomCompleted) {
                mainDoor.CloseDoor();
                mainDoor.LockMode(true);
                if (!playedFirstAudio) {
                    initialSound.PlayAudio();
                    playedFirstAudio = true;
                }
                enableLights = true;
                SetNextTarget();
                liveShootingActive = true;
            }
        }
    }
}
