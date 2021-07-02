using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System.Linq;

public class DoorController : MonoBehaviour {

    [Header("Door Settings")]
    [Tooltip("Enter Tag names to allow objects to pass through the door.")]
    [SerializeField] List<string> tagsAllowed = new List<string> { "Player", "Enemy" };
    [Tooltip("Enable if you wish the door to stay Lock.")]
    [SerializeField] bool lockDoor = false;

    [HideInInspector] public bool interactable;
    [HideInInspector] public KeyCode inputKey = KeyCode.E;
    [HideInInspector] public bool isDoubleDoor = false;
    [HideInInspector] public bool openSideWays = false;

    private List<GameObject> inside = new List<GameObject>();

    private Animator anim;
    private bool isDoorOpen = false;

    private void Start() {
        anim = GetComponent<Animator>();
    }


    private void OnTriggerStay(Collider other) {
        if (!lockDoor) {
            if (!isDoorOpen) {
                if (tagsAllowed.Contains(other.gameObject.tag)) {
                    if (interactable) {
                        if (Input.GetKey(KeyCode.E)) {
                            isDoorOpen = true;
                            inside.Add(other.gameObject);
                            return;
                        }
                    } else {
                        isDoorOpen = true;
                        inside.Add(other.gameObject);
                        return;
                    }
                }
            } else {
                if (tagsAllowed.Contains(other.gameObject.tag)) {
                    if (!inside.Contains(other.gameObject)) {
                        inside.Add(other.gameObject);
                        return;
                    }

                }

            }
            if (isDoubleDoor) {
                anim.SetBool("side", openSideWays);
            }
            anim.SetBool("Open", isDoorOpen);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (!lockDoor) {
            if (inside.Count > 0 && isDoorOpen) {
                if (tagsAllowed.Contains(other.gameObject.tag)) {
                    inside.Remove(other.gameObject);
                }

            }
            if (inside.Count == 0) {
                isDoorOpen = false;
                anim.SetBool("side", openSideWays);
                anim.SetBool("Open", isDoorOpen);
            }
        }
    }

    /// <summary>
    /// Function that Sets the door lock system.
    /// </summary>
    /// <param name="value">True, The door will stay lock. | False, the door is unlocked.</param>
    public void LockMode(bool value) {
        lockDoor = value;
    }

}