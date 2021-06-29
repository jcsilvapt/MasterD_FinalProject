using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DoorController : MonoBehaviour {

    [Header("Door Settings")]
    [Tooltip("Enable if you wish the door to stay Lock.")]
    [SerializeField] bool locked;
    [SerializeField] bool openSideWays = false;
    [Tooltip("Enter Tag names to allow objects to pass through the door.")]
    [SerializeField] List<string> tags = new List<string> { "Player", "Enemy" };

    private List<GameObject> inside = new List<GameObject>();

    private Animator anim;
    private bool isDoorOpen = false;

    private void Start() {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other) {
        if (!locked) {
            if (!isDoorOpen) {
                if (tags.Contains(other.gameObject.tag)) {
                    isDoorOpen = true;
                    anim.SetBool("side", openSideWays);
                    anim.SetBool("Open", isDoorOpen);
                    inside.Add(other.gameObject);
                    return;
                }
            } else {
                if (tags.Contains(other.gameObject.tag)) {
                    if (!inside.Contains(other.gameObject)) {
                        anim.SetBool("Open", isDoorOpen);
                        inside.Add(other.gameObject);
                        return;
                    }

                }

            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (!locked) {
            if (inside.Count > 0 && isDoorOpen) {
                if (tags.Contains(other.gameObject.tag)) {
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
        locked = value;
    }

}
