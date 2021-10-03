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
    [Tooltip("Set False so there will be no Signs on the door")]
    [SerializeField] bool hasSigns = true;
    [SerializeField] List<GameObject> openSign = new List<GameObject>();
    [SerializeField] List<GameObject> closeSign = new List<GameObject>();

    [HideInInspector] public bool interactable;
    [HideInInspector] public KeyCode inputKey = KeyCode.E;
    [HideInInspector] public bool isDoubleDoor = false;
    [HideInInspector] public bool openSideWays = false;

    private List<GameObject> inside = new List<GameObject>();

    private Animator anim;
    private bool isDoorOpen = false;
    private bool lastDoorStatus = false;
    public AudioSource doorSoundsSource;
    public AudioClip doorSounds;

    #region Fabio Changes

    [SerializeField] private bool startsOpen;

    [SerializeField] private bool isAlwaysOpen;

    #endregion


    private void Start() {
        anim = GetComponent<Animator>();
        if (hasSigns) {
            ChangeDoorStatusSign();
        } else {
            HideSignOnDoors();
        }
    }

    private void Update() {

        if (!hasSigns) {
            return;
        }
        if (lockDoor == lastDoorStatus) {
            return;
        }

        ChangeDoorStatusSign();

    }


    private void OnTriggerStay(Collider other) {

        //Debug.Log(other.transform.name);

        if (isAlwaysOpen)
        {
            return;
        }

        if (startsOpen) {
            return;
        }

        if (!lockDoor) {
            if (!isDoorOpen) {
                if (tagsAllowed.Contains(other.gameObject.tag)) {
                    if (interactable) {
                        if (Input.GetKey(KeyCode.E)) {
                            isDoorOpen = true;
                            //DoorSound();
                            inside.Add(other.gameObject);
                            return;
                        }
                    } else {
                        isDoorOpen = true;
                        inside.Add(other.gameObject);
                        //DoorSound();
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
       
        if (isAlwaysOpen)
        {
            return;
        }
        
        if (startsOpen) {
            return;
        }

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
                //DoorSound();
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

    private void ChangeDoorStatusSign() {
        for (int i = 0; i < openSign.Count; i++) {
            openSign[i].SetActive(!lockDoor);
            closeSign[i].SetActive(lockDoor);
        }
        lastDoorStatus = lockDoor;
    }

    private void HideSignOnDoors() {
        for (int i = 0; i < openSign.Count; i++) {
            openSign[i].SetActive(false);
            closeSign[i].SetActive(false);
        }
    }

    private void DoorSound()
    {
        doorSoundsSource.PlayOneShot(doorSounds);
    }
    #region Fabio Changes

    public void CloseDoor() {
        anim.SetBool("CloseDoor", true);
        anim.SetBool("Open", false);
        //anim.runtimeAnimatorController = ;
        startsOpen = false;
    }

    public void SetAlwaysOpen()
    {
        anim.SetBool("Open", true);

        isAlwaysOpen = true;
    }

    #endregion
}