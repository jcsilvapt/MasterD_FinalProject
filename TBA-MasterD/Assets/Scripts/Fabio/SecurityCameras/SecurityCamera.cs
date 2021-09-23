using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    #region References

    //Second Level Manager Reference
    [SerializeField] private SecondLevelManager secondLevelManager;

    //Second Level AI Manager Reference
    [SerializeField] private Fabio_AIManager aiManager;

    //Enemies References
    [SerializeField] private Fabio_EnemySecondLevel[] enemies;

    //Security Camera Glass Reference
    private Transform cameraGlass;

    //More Security Cameras to Disable
    [SerializeField] private SecurityCamera[] securityCameras;

    //Associated Door
    [SerializeField] private DoorController door;

    #endregion

    #region Movement Variables

    private bool isActive;

    [SerializeField] private float minimumRotationValue;
    [SerializeField] private float maximumRotationValue;

    [SerializeField] private float rotationSpeed;

    #endregion


    private void Start()
    {
        cameraGlass = transform.parent;

        isActive = true;
    }

    private void Update()
    {
        if (isActive)
        {
            cameraGlass.localEulerAngles = Vector3.right * ((((maximumRotationValue - minimumRotationValue) / 2) * Mathf.Sin(rotationSpeed * Time.time)) + ((minimumRotationValue + maximumRotationValue) / 2));
        }
    }

    private void EnemyDetected()
    {
        foreach(Fabio_EnemySecondLevel enemy in enemies)
        {
            aiManager.PlayerDetected();
        }

        foreach (SecurityCamera camera in securityCameras)
        {
            camera.DeactivateLight();
        }

        DeactivateLight();
        secondLevelManager.OpenDoor(door);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            EnemyDetected();
        }
    }

    public void DeactivateLight()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<MeshCollider>().enabled = false;

        isActive = false;
    }
}
