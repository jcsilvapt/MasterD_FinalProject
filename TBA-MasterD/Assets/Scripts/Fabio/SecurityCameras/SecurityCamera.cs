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

    //Security Camera Glass Reference
    private Transform cameraGlass;

    //More Security Cameras to Disable
    [SerializeField] private SecurityCamera[] securityCameras;

    //Associated Door
    [SerializeField] private DoorController door;

    private Subtitles subtitleSystem;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private string[] subtitles;

    #endregion

    #region Movement Variables

    private bool isActive;

    [SerializeField] private float minimumRotationValue;
    [SerializeField] private float maximumRotationValue;

    [SerializeField] private float rotationSpeed;

    #endregion


    private void Start()
    {
        subtitleSystem = GetComponent<Subtitles>();

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
        aiManager.PlayerDetected();

        foreach (SecurityCamera camera in securityCameras)
        {
            camera.DeactivateLight();
        }

        DeactivateLight();
        secondLevelManager.OpenDoor(door);

        int random = SetRandomVoiceLineAndSubtitles();
        subtitleSystem.SetAudioAndSubtitles(DefineAudioClip(random), " ", null);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, (other.transform.position - transform.position), out hit)){
                if (hit.transform.CompareTag("PlayerParent"))
                {
                    EnemyDetected();
                }
            }
            Debug.Log(hit.transform.tag);
        }

    }

    public void DeactivateLight()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<MeshCollider>().enabled = false;

        isActive = false;
    }

    private int SetRandomVoiceLineAndSubtitles()
    {
        return Random.Range(0, audioClips.Length);
    }

    private AudioClip DefineAudioClip(int random)
    {
        return audioClips[random];
    }

    private string DefineSubtitles(int random)
    {
        return subtitles[random];
    }
}
