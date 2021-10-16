using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShootingCraneController : MonoBehaviour {

    [SerializeField] GameObject _UI;
    [SerializeField] GameObject _UIText;

    [SerializeField] Transform crane;
    [SerializeField] Transform distancesTransform;
    [SerializeField] float craneSpeed = 1f;
    private List<Transform> distances = new List<Transform>();

    private Transform selectedDistance;

    public charController player;
    public cameraRotation pCam;

    private bool isMoving = false;
    private bool isUIActive = false;
    private bool hasPlayer = false;

    private void Start() {

        TutorialLevelManager.ins.triggerAction += ResetTarget;

        _UI.SetActive(false);
        _UIText.SetActive(false);
        foreach (Transform d in distancesTransform) {
            distances.Add(d);
        }
    }

    private void OnDisable() {
        TutorialLevelManager.ins.triggerAction -= ResetTarget;
    }

    void ResetTarget() {
        selectedDistance = distances[3];
        isMoving = true;
        TutorialLevelManager.ins.triggerAction -= ResetTarget;
    }

    public void SetTargetDistance(int dist) {
        selectedDistance = distances[dist];
        isMoving = true;
        _UI.SetActive(false);
        isUIActive = false;
        player.StartMovement();
        pCam.StartMouseMovement();
        player.UnlockPause();
        GameManager.SetCursorVisibility(false);
    }

    public void CloseDistance() {
        _UI.SetActive(false);
        isUIActive = false;
        player.StartMovement();
        pCam.StartMouseMovement();
        player.UnlockPause();
        GameManager.SetCursorVisibility(false);
    }

    private void Update() {
        if (hasPlayer) {
            if (!GameManager.GetPause()) {
                if (!isUIActive) {
                    if (Input.GetKeyDown(KeyMapper.inputKey.Interaction)) {
                        player.LockPause();
                        player.StopMovement();
                        pCam.StopMouseMovement();
                        _UI.SetActive(true);
                        isUIActive = true;
                        GameManager.SetCursorVisibility(true);
                    }
                } else {
                    if (Input.GetKeyDown(KeyMapper.inputKey.Interaction)) {
                        player.StartMovement();
                        pCam.StartMouseMovement();
                        _UI.SetActive(false);
                        isUIActive = false;
                        GameManager.SetCursorVisibility(false);
                        player.UnlockPause();
                    }
                }
            }
        }

        if (isMoving) {
            crane.position = Vector3.MoveTowards(crane.position, selectedDistance.position, Time.deltaTime * craneSpeed);
            if (crane.position == selectedDistance.position) isMoving = false;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player") || other.CompareTag("PlayerParent")) {
            player = other.transform.parent.parent.GetComponent<charController>();
            pCam = other.transform.parent.parent.GetComponentInChildren<cameraRotation>();
            _UIText.GetComponent<TMP_Text>().text = "Press '" + KeyMapper.inputKey.Interaction.ToString() + "' to edit target distance";
            _UIText.SetActive(true);
            hasPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        _UI.SetActive(false);
        _UIText.SetActive(false);
        isUIActive = false;
        hasPlayer = false;
        player.StartMovement();
        player.UnlockPause();
        player = null;
        pCam.StartMouseMovement();
        pCam = null;
        if (GameManager.GetCursorVisibility())
            GameManager.SetCursorVisibility(false);
    }
}
