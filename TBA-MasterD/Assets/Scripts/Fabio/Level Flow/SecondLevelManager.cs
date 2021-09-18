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

    public void UnlockDoorElevatorControlRoom()
    {
        
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

}
