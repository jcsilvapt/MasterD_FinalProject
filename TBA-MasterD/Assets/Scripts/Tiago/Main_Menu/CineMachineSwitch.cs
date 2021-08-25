using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
public class CineMachineSwitch : MonoBehaviour
{
    // Virtual Cameras
    public CinemachineVirtualCamera vCam1;
    public CinemachineVirtualCamera vCam2;
    public CinemachineVirtualCamera vCam3;
    public CinemachineVirtualCamera vCam4;

    //reset position to enemy running    
    public Transform enemy;
    public Vector3 startPosition;


    public void Start()
    {
        vCam1.Priority = 10;
        vCam2.Priority = 0;
        vCam3.Priority = 0;
        vCam4.Priority = 0;

       startPosition = enemy.position;
    }

    public void SwitchCineCamera1()
    {
        vCam1.Priority = 10;
        vCam2.Priority = 0;
        vCam3.Priority = 0;
        vCam4.Priority = 0;

        ResetPosition();
    }
    public void SwitchCineCamera2()
    {
        vCam1.Priority = 0;
        vCam2.Priority = 10;
        vCam3.Priority = 0;
        vCam4.Priority = 0;
    }
    public void SwitchCineCamera3()
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
    }

    public void ResetPosition()
    {

        enemy.position = startPosition;
      
    }

   
}
