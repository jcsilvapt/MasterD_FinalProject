using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryElevatorScript : MonoBehaviour {

    [SerializeField] float timeToOpenDoor;

    [SerializeField] Modal modal;

    [SerializeField] AudioSource leftDoor;
    [SerializeField] AudioSource rightDoor;


    [Header("Developer")]
    [SerializeField] Animator animator;


    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
        StartCoroutine(OpenDoorDelay(timeToOpenDoor));
    }

    IEnumerator OpenDoorDelay(float time) {
        yield return new WaitForSeconds(time);
        leftDoor.Play();
        rightDoor.Play();
        animator.SetBool("Open", true);
        animator.SetBool("Close", false);
        //modal.ShowModal();
    }
}
