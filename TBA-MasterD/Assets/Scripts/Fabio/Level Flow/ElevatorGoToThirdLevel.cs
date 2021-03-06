using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorGoToThirdLevel : MonoBehaviour
{
    #region References

    //Second Level Manager
    [SerializeField] private SecondLevelManager levelManager;

    //Elevator Controller Reference
    [SerializeField] private ElevatorController elevator;

    //Array of AI Manager References
    [SerializeField] private Fabio_AIManager[] aiManagers;

    //SubtitleSystem
    private Subtitles subtitleSystem;
    [SerializeField] private AudioClip voiceLine;
    [SerializeField] private string subtitles;

    #endregion

    #region Control Variables

    [SerializeField] private bool canClose;

    private int howManyAIManagersAreWorking;

    private bool isSecondLevelActive;

    #endregion

    private void Start()
    {
        subtitleSystem = GetComponent<Subtitles>();

        isSecondLevelActive = true;
    }

    private void Update()
    {
        if(!isSecondLevelActive){
            return;
        }

        howManyAIManagersAreWorking = 0;

        foreach (Fabio_AIManager manager in aiManagers)
        {
            if (manager.GetIsAIManagerWorking())
            {
                howManyAIManagersAreWorking++;
                break;
            }
        }

        if(howManyAIManagersAreWorking > 0)
        {
            canClose = false;
        }
        else
        {
            canClose = true;
        }
    }

    public void DoorsOpened()
    {
        GameObject.FindGameObjectWithTag("PlayerParent").GetComponent<charController>().StartMovement();
    }

    private void OnTriggerStay(Collider other)
    {
        if(!isSecondLevelActive) {
            return;
        }

        if (!canClose)
        {
            return;
        }

        if (canClose)
        {   
            if (other.tag == "Player")
            {
                levelManager.SetElevatorAudio();
                other.transform.parent.parent.GetComponent<charController>().StopMovement();
                elevator.CloseElevatorDoor();
                isSecondLevelActive = false;
                subtitleSystem.SetAudioAndSubtitles(voiceLine, subtitles, null);
            }
        }
    }
}
