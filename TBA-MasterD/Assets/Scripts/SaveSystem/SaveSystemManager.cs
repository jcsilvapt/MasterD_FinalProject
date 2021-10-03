using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemManager : MonoBehaviour {

    public static SaveSystemManager ins;

    private List<Enemy> enemiesInScene = new List<Enemy>();

    [SerializeField] SO_PlayerData playerProfile;
    private SaveSystem saveSystem;


    private void Awake() {
        if (ins == null) {
            ins = this;
            DontDestroyOnLoad(this);
            saveSystem = new SaveSystem();
            GetInitialData();
        } else {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Method that gets the current saved Data.
    /// </summary>
    private void GetInitialData() {
        if(HasSavedData()) {
            Debug.Log("initialData");
            playerProfile = saveSystem.Load(playerProfile);
        }
    }

    /// <summary>
    /// Method that Saves the current Game State
    /// </summary>
    /// <param name="objectsToBeSaved">Usual is the List of the enemies on the scene</param>
    private void SaveGame(List<GameObject> objectsToBeSaved = null) {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        charController charPlayer = player.GetComponent<charController>();
        Debug.Log("Player Location: " + player.transform.position);
        playerProfile.currentPosition = player.transform.position;
        //playerProfile.currentRotation = Quaternion.Euler(player.transform.Rota);
        playerProfile.hasDrone = charPlayer.canUseDrone;
        playerProfile.hasWeapon = charPlayer.hasWeapon;
        saveSystem.Save(playerProfile);

        if(objectsToBeSaved != null) {
            foreach (GameObject b in objectsToBeSaved) {
                enemiesInScene.Add(b.GetComponent<Enemy>());
            }
        }
        Debug.Log("Saving Game...");
    }

    /// <summary>
    /// Method that Loads current location information
    /// </summary>
    /// <param name="objectsToBeLoaded">List of the enemies in the current scene (CheckPoint Only)</param>
    /// <returns></returns>
    private List<GameObject> LoadGameData(List<GameObject> objectsToBeLoaded = null) {        
        playerProfile = saveSystem.Load(playerProfile);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        charController charPlayer = player.GetComponent<charController>();
        player.transform.position = playerProfile.currentPosition;
        //player.transform.rotation = playerProfile.currentRotation;
        charPlayer.canUseDrone = playerProfile.hasDrone;
        charPlayer.hasWeapon = playerProfile.hasWeapon;

        Debug.Log("Loading Game...");

        if(objectsToBeLoaded != null) {
            int counter = 0;
            foreach (GameObject b in objectsToBeLoaded) {
                Enemy temp = b.GetComponent<Enemy>();
                temp = enemiesInScene[counter];
                counter++;
            }
            return objectsToBeLoaded;
        }
        return null;
    }

    private void LoadGame() {
        GameManager.ChangeScene(playerProfile.currentScene, true);
    }

    public static void Save(List<GameObject> objectsToBeSaved = null) {
        if (ins != null) {
            ins.SaveGame(objectsToBeSaved);
        }
    }

    /// <summary>
    /// Method that resets the Game Data (on the Load)
    /// </summary>
    /// <param name="objectsToBeLoaded"></param>
    /// <returns></returns>
    public static List<GameObject> LoadData(List<GameObject> objectsToBeLoaded = null) {
        if (ins != null) {
            return ins.LoadGameData(objectsToBeLoaded);
        }
        return null;
    }

    /// <summary>
    /// Method that Loads the game (full-restart)
    /// </summary>
    public static void Load() {
        if(ins != null) {
            ins.LoadGame();
        }
    }
        
    public static bool HasSavedData() {
        if (ins != null) {
            return ins.saveSystem.HasSavedData();
        }
        return false;
    }

    public static int GetCurrentSaveScene() {
        if(ins != null) {
            return ins.playerProfile.currentScene;
        }
        return -1;
    }
}
