﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneController : MonoBehaviour, IDamage {

    [Tooltip("Add Here all the moving parts objects in the scene.")]
    [SerializeField] GameObject[] movingParts;
    [Tooltip("Add the GameObjects Tag that are allowed to use this.")]
    [SerializeField] List<string> allowedTagsToInteract = new List<string> { "Player" };

    [SerializeField] bool interactionWithBullet = false;
    [SerializeField] bool isActive = false;

    [SerializeField] KeyCode keyToInteract;


    private void Start() {
        if (!isActive) {
            ShutDownCrane();
        } else {
            ActivateCrane();
        }
    }


    private void ActivateCrane() {
        foreach (GameObject g in movingParts) {
            Animator b = g.GetComponent<Animator>();
            b.enabled = true;
        }
        isActive = true;
    }

    private void ShutDownCrane() {
        foreach (GameObject g in movingParts) {
            Animator b = g.GetComponent<Animator>();
            b.enabled = false;
        }
        isActive = false;
    }

    private void OnTriggerStay(Collider other) {
        if (!isActive) {
            if (allowedTagsToInteract.Contains(other.gameObject.tag)) {
                if (Input.GetKeyDown(keyToInteract)) {
                    ActivateCrane();
                }
            }
        }

    }

    private void OnCollisionEnter(Collision other) {
        if (interactionWithBullet) {
            if (!isActive) {
                foreach (string tag in allowedTagsToInteract) {
                    if (other.gameObject.tag == tag) {
                        ActivateCrane();
                    }
                }
            }
        }
    }

    public void TakeDamage() {
        Debug.Log("I WORK SOMEHOW...");
        ActivateCrane();
    }
}
