using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CineMachine2 : MonoBehaviour
{
    // Virtual Cameras
    public CinemachineVirtualCamera vCam1;
    public CinemachineVirtualCamera vCam2;
    public CinemachineVirtualCamera vCam3;
    //  public CinemachineVirtualCamera vCam4;

    public void Start()
    {
        vCam1.Priority = 10;
        vCam2.Priority = 0;
        vCam3.Priority = 0;
    }
    public void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            CutBlende();
        }
    }

    public void SwitchCineCamera1()
    {
        vCam1.Priority = 10;
        vCam2.Priority = 0;
        vCam3.Priority = 0;
        //  vCam4.Priority = 0;
    }
    public void SwitchCineCamera2()
    {
        vCam1.Priority = 0;
        vCam2.Priority = 10;
        vCam3.Priority = 0;
        //  vCam4.Priority = 0;
        StartCoroutine(SwitchTimer());
    }
    /* public void SwitchCineCamera3()
     {
         vCam1.Priority = 0;
         vCam2.Priority = 0;
         vCam3.Priority = 10;
         vCam4.Priority = 0;
     }
     public void SwitchCineCamera4()
     {
         vCam1.Priority = 0;
         vCam2.Priority = 0;
         vCam3.Priority = 0;
         vCam4.Priority = 10;
     }*/


    public IEnumerator SwitchTimer()
    {
        yield return new WaitForSeconds(12);
        vCam1.Priority = 0;
        vCam2.Priority = 0;
        vCam3.Priority = 10;
    }

    public void CutBlende()
    {
        vCam1.Priority = 0;
        vCam2.Priority = 0;
        vCam3.Priority = 10;
       
    }
}
