using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garage_Manager : MonoBehaviour
{
    [Header("Barrier")]

    public GameObject barrier;
    private Animator animBarr;

    [Header("Glass")]
    public GameObject normalGlass;
    public GameObject brokenGlass;

    void Start()
    {
        animBarr = barrier.GetComponent<Animator>();
    }


    void Update()
    {
    }

    #region Barrier Rotation
    public void RotateBarrier()
    {
        animBarr.SetTrigger("isRotate");
    }

    #endregion

    #region Breaking Glass
    public void switchGlass()
    {
        normalGlass.active = false;
        brokenGlass.active = true;
    }

    #endregion
}
