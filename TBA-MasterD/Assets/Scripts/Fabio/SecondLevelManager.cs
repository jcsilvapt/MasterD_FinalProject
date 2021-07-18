using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondLevelManager : MonoBehaviour
{
    //Singleton Instance
    public static SecondLevelManager instance;

    //Current Enemy Wave
    [SerializeField] private int enemyWave;

    #region Enemy Spawn

    //Number of Enemies per Room
    [SerializeField] private int numberOfEnemies;

    //Enemies Instance Spawn Points
    [SerializeField] private Transform[] instanceSpawnPoints;

    #endregion

    #region Door References

    [SerializeField] private DoorController centerRoomDoor;
    
    [SerializeField] private DoorController[] instanceEnemiesDoors;

    [SerializeField] private DoorController[] roomsEnemiesDoors;

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

    private void Start()
    {
        enemyWave = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            FinishEnemyWaves();
        }
    }

    public void StartEnemyWaves()
    {
        if(enemyWave == 0)
        {
            centerRoomDoor.LockMode(true);
        }

        enemyWave++;

        //Lock Room Doors
        foreach (DoorController door in roomsEnemiesDoors)
        {
            door.LockMode(true);
        }

        //Spawn Enemies
        SpawnEnemies();

        //Unlock Instantiation Room Doors
        foreach (DoorController door in instanceEnemiesDoors)
        {
            door.LockMode(false);
        }
        
        //Move Enemies to Rooms
        //Lock Intantiation Rooms and Unlock Door Rooms
    }

    public void FinishEnemyWaves()
    {
        if (enemyWave > 3)
        {
            centerRoomDoor.LockMode(false);
            //Call Elevator
            return;
        }

        StartEnemyWaves();
    }

    private void SpawnEnemies()
    {
        for (int index = 0; index < numberOfEnemies; index++)
        {
            foreach (Transform spawnLocation in instanceSpawnPoints)
            {
                Debug.Log("Instantiated Enemy");
            }
        }
    }
}
