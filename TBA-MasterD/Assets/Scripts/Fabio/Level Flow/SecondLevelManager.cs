using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondLevelManager : MonoBehaviour
{
    //Singleton Instance
    public static SecondLevelManager instance;

    #region References

    #region Doors

    [SerializeField] private DoorController doorFirstToSecondLevel;
    [SerializeField] private DoorController doorSecondLevelEntrance;
    [SerializeField] private DoorController doorRoom1WC;
    [SerializeField] private DoorController doorRoom2Lab20;
    [SerializeField] private DoorController doorRoom3Lab21;
    [SerializeField] private DoorController doorRoom4ControlRoomRight;
    [SerializeField] private DoorController doorRoom4ControlRoomLeft;
    [SerializeField] private DoorController doorRoom5Cafeteria;
    [SerializeField] private DoorController doorRoom6Lab22;
    [SerializeField] private DoorController doorSecondLevelExit;
    [SerializeField] private DoorController doorElevatorControlRoom;
    [SerializeField] private DoorController doorResourcesRoom; //To be created and added

    #endregion

    #region Sound Sources

    [SerializeField] private GameObject normalMusic;
    [SerializeField] private GameObject actionMusic;
    [SerializeField] private GameObject easterEggMusic;
    [SerializeField] private GameObject elevatorMusic;

    #endregion

    #region AI Managers

    [SerializeField] private Fabio_AIManager[] aiManagers;

    #endregion

    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        normalMusic.SetActive(false);
        actionMusic.SetActive(false);
        easterEggMusic.SetActive(false);
        elevatorMusic.SetActive(false);
    }

    private void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("PlayerParent");

        if (!player.activeSelf) {
            player.SetActive(true);
        }

        normalMusic.SetActive(false);
        actionMusic.SetActive(false);
        easterEggMusic.SetActive(false);
        elevatorMusic.SetActive(false);
    }

    public void OpenDoor(DoorController door)
    {
        door.LockMode(false);
        door.SetAlwaysOpen();
    }
    
    #region Door

    public void LockDoorFirstToSecondLevel()
    {
        doorFirstToSecondLevel.LockMode(true);
    }

    public void UnlockDoorFirstToSecondLevel()
    {
        doorFirstToSecondLevel.LockMode(false);
    }

    public void UnlockDoorSecondLevelExit()
    {
        doorSecondLevelExit.LockMode(false);
    }

    public void UnlockDoorResourcesRoom()
    {
        doorResourcesRoom.LockMode(false);
        doorResourcesRoom.SetAlwaysOpen();
    }

    #endregion

    #region Sounds

    public void SetNormalAudio()
    {
        foreach (Fabio_AIManager aiManager in aiManagers)
        {
            if (aiManager.GetIsAIManagerWorking())
            {
                return;
            }
        }

        actionMusic.SetActive(false);
        easterEggMusic.SetActive(false);
        elevatorMusic.SetActive(false);
        normalMusic.SetActive(true);
    }

    public void SetActionAudio()
    {
        normalMusic.SetActive(false);
        easterEggMusic.SetActive(false);
        elevatorMusic.SetActive(false);
        actionMusic.SetActive(true);
    }

    public void SetEasterEggAudio()
    {
        normalMusic.SetActive(false);
        actionMusic.SetActive(false);
        elevatorMusic.SetActive(false);
        easterEggMusic.SetActive(true);
    }

    public void SetElevatorAudio()
    {
        normalMusic.SetActive(false);
        actionMusic.SetActive(false);
        easterEggMusic.SetActive(false);
        elevatorMusic.SetActive(true);
    }

    public void SetAudioToPlay()
    {
        foreach (Fabio_AIManager aiManager in aiManagers)
        {
            if (aiManager.GetIsAIManagerWorking())
            {
                normalMusic.SetActive(false);
                easterEggMusic.SetActive(false);
                elevatorMusic.SetActive(false);
                actionMusic.SetActive(true);

                return;
            }
        }

        actionMusic.SetActive(false);
        easterEggMusic.SetActive(false);
        elevatorMusic.SetActive(false);
        normalMusic.SetActive(true);
    }

    #endregion
}
