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

    //Array of AI Manager References
    [SerializeField] private Fabio_AIManager[] aiManager;

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

    public void CloseElevatorDoor()
    {
        animator.SetBool("Close", true);
        animator.SetBool("Open", false);
    }
}
