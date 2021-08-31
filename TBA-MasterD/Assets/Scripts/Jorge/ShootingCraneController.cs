using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingCraneController : MonoBehaviour {

    [SerializeField] GameObject _UI;
    [SerializeField] GameObject _UIText;

    [SerializeField] Transform crane;
    [SerializeField] Transform distancesTransform;
    [SerializeField] float craneSpeed = 1f;
    private List<Transform> distances = new List<Transform>();

    private Transform selectedDistance;

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
        GameManager.SetCursorVisibility();
    }

    private void Update() {
        if (hasPlayer) {
            if (!isUIActive) {
                if (Input.GetKeyDown(KeyMapper.inputKey.Interaction)) {
                    _UI.SetActive(true);
                    isUIActive = true;
                    GameManager.SetCursorVisibility();
                }
            } else {
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    _UI.SetActive(false);
                    isUIActive = false;
                    GameManager.SetCursorVisibility();
                }
            }
        }

        if (isMoving) {
            crane.position = Vector3.MoveTowards(crane.position, selectedDistance.position, Time.deltaTime * craneSpeed);
            if (crane.position == selectedDistance.position) isMoving = false;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.transform.tag == "Player") {
            _UIText.SetActive(true);
            hasPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        _UI.SetActive(false);
        _UIText.SetActive(false);
        isUIActive = false;
        hasPlayer = false;
        if (GameManager.GetCursorVisibility())
            GameManager.SetCursorVisibility();
    }
}
