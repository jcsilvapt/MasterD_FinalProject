using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryElevatorScript : MonoBehaviour {

    [SerializeField] float timeToOpenDoor;

    [SerializeField] Modal modal;
    [SerializeField] AudioSource aSource;

    [SerializeField] AudioClip elevatorSound;
    [SerializeField] AudioClip elevatorDoors;


    [Header("Developer")]
    [SerializeField] Animator animator;


    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
        aSource = GetComponent<AudioSource>();
        aSource.clip = elevatorSound;
        aSource.Play();
        StartCoroutine(OpenDoorDelay(timeToOpenDoor));
    }

    IEnumerator OpenDoorDelay(float time) {
        yield return new WaitForSeconds(time / 2);
        aSource.Stop();
        yield return new WaitForSeconds(time);
        aSource.clip = elevatorDoors;
        aSource.Play();
        animator.SetBool("Open", true);
        animator.SetBool("Close", false);
        modal.ShowModal();
    }
}
