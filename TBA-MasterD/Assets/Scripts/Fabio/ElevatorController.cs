using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    #region References

    //Animator Reference
    private Animator animator;

    //Signs GameObjects References
    [SerializeField] private GameObject signAvailable;
    [SerializeField] private GameObject signUnavailable;

    // Player Reference
    [SerializeField] private Transform player = null;

    #endregion

    #region Control Variables

    #endregion

    private void Start()
    {
        animator = GetComponent<Animator>();

        signUnavailable.SetActive(true);
    }

    public void SetAvailability(bool isAvailable)
    {
        signAvailable.SetActive(isAvailable);
        signUnavailable.SetActive(!isAvailable);

        animator.SetBool("Open", isAvailable);
        animator.SetBool("Close", !isAvailable);
    }

    public void ElevatorDoorsOpen()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<charController>().StartMovement();
    }

    public void CloseElevatorDoor()
    {
        animator.SetBool("Close", true);
        animator.SetBool("Open", false);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            player = other.transform.parent.parent;
            player.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")) {
            player.parent = null;
            player = null;
        }
    }
}
