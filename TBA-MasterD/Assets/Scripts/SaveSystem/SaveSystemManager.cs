using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemManager : MonoBehaviour {

    public static SaveSystemManager ins;

    [SerializeField] List<Fabio_EnemySecondLevel> enemiesInSceneFabio = new List<Fabio_EnemySecondLevel>();
    [SerializeField] List<Enemy> enemiesInSceneTiago = new List<Enemy>();

    [SerializeField] SO_PlayerData playerProfile;
    [SerializeField] Checkpoint cpoint = null;
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
        if (HasSavedData()) {
            playerProfile = saveSystem.Load(playerProfile);
            LoadGameSettings();
        }
    }

    /// <summary>
    /// Method that Saves the current Game State
    /// </summary>
    /// <param name="objectsToBeSaved">Usual is the List of the enemies on the scene</param>
    private void SaveGame(Checkpoint checkpoint = null, List<GameObject> objectsToBeSaved = null) {

        GameObject player = GameObject.FindGameObjectWithTag("PlayerParent");
        charController charPlayer = player.GetComponent<charController>();

        playerProfile.currentPosition = player.transform.position;

        playerProfile.hasDrone = charPlayer.canUseDrone;
        playerProfile.hasWeapon = charPlayer.hasWeapon;
        playerProfile.currentScene = checkpoint.GetCurrentSceneIndex();
        saveSystem.Save(playerProfile);

        if (checkpoint != null) cpoint = checkpoint;

        if (objectsToBeSaved != null) {
            foreach (GameObject b in objectsToBeSaved) {
                if (b.GetComponent<Enemy>() != null) {
                    enemiesInSceneTiago.Add(b.GetComponent<Enemy>());
                } else {
                    enemiesInSceneFabio.Add(b.GetComponent<Fabio_EnemySecondLevel>());
                }
            }
        }
    }

    /// <summary>
    /// Method that Loads current location information
    /// </summary>
    /// <param name="objectsToBeLoaded">List of the enemies in the current scene (CheckPoint Only)</param>
    /// <returns></returns>
    private void LoadGameData() {
        playerProfile = saveSystem.Load(playerProfile);
        GameObject player = GameObject.FindGameObjectWithTag("PlayerParent");
        charController charPlayer = player.GetComponent<charController>();
        player.transform.position = playerProfile.currentPosition;
        //player.transform.rotation = playerProfile.currentRotation;
        charPlayer.canUseDrone = playerProfile.hasDrone;
        charPlayer.hasWeapon = playerProfile.hasWeapon;
        Debug.Log("Loading Game...");
    }

    private List<GameObject> UpdateEnemiesInScene(List<GameObject> objectsToBeLoaded) {
        if (objectsToBeLoaded != null) {
            int counter = 0;
            foreach (GameObject b in objectsToBeLoaded) {
                if (b.GetComponent<Enemy>() != null) {
                    Enemy temp = b.GetComponent<Enemy>();
                    temp = enemiesInSceneTiago[counter];
                } else {
                    Fabio_EnemySecondLevel temp = b.GetComponent<Fabio_EnemySecondLevel>();
                    temp = enemiesInSceneFabio[counter];

                }
                counter++;
            }
            return objectsToBeLoaded;
        }
        return null;
    }

    private void LoadGameSettings() {
        // Set Input Settings
        KeyMapper.inputKey.WalkFoward = playerProfile.walkFoward;
        KeyMapper.inputKey.WalkBackwards = playerProfile.walkBackwards;
        KeyMapper.inputKey.WalkLeft = playerProfile.walkLeft;
        KeyMapper.inputKey.WalkRight = playerProfile.walkRight;
        KeyMapper.inputKey.Crouch = playerProfile.crouch;
        KeyMapper.inputKey.Jump = playerProfile.jump;
        KeyMapper.inputKey.Sprint = playerProfile.sprint;
        KeyMapper.inputKey.DroneActivation = playerProfile.droneActivation;
        KeyMapper.inputKey.DroneMoveUp = playerProfile.droneMoveUp;
        KeyMapper.inputKey.DroneMoveDown = playerProfile.droneMoveDown;
        KeyMapper.inputKey.Interaction = playerProfile.interaction;
        KeyMapper.inputKey.Escape = playerProfile.escape;

        KeyMapper.inputKey.MouseSensitivity = playerProfile.mouseSensitivity;
        KeyMapper.inputKey.InvertMouse = playerProfile.invertMouse;
    }

    private void SaveGameSettings() {
        playerProfile.walkFoward = KeyMapper.inputKey.WalkFoward;
        playerProfile.walkBackwards = KeyMapper.inputKey.WalkBackwards;
        playerProfile.walkLeft = KeyMapper.inputKey.WalkLeft;
        playerProfile.walkRight = KeyMapper.inputKey.WalkRight;
        playerProfile.crouch = KeyMapper.inputKey.Crouch;
        playerProfile.jump = KeyMapper.inputKey.Jump;
        playerProfile.sprint = KeyMapper.inputKey.Sprint;
        playerProfile.droneActivation = KeyMapper.inputKey.DroneActivation;
        playerProfile.droneMoveUp = KeyMapper.inputKey.DroneMoveUp;
        playerProfile.droneMoveDown = KeyMapper.inputKey.DroneMoveDown;
        playerProfile.interaction = KeyMapper.inputKey.Interaction;
        playerProfile.escape = KeyMapper.inputKey.Escape;

        playerProfile.mouseSensitivity = KeyMapper.inputKey.MouseSensitivity;
        playerProfile.invertMouse = KeyMapper.inputKey.InvertMouse;

        saveSystem.Save(playerProfile);
    }

    private void LoadGame() {
        GameManager.ChangeScene(playerProfile.currentScene, true);
        if (cpoint != null) {
            List<GameObject> temp = UpdateEnemiesInScene(cpoint.GetEnemiesInScene());
            cpoint.SetEnemiesStats(temp);
        }
    }

    public static void Save(Checkpoint checkpoint = null, List<GameObject> objectsToBeSaved = null) {
        if (ins != null) {
            ins.SaveGame(checkpoint, objectsToBeSaved);
        }
    }

    /// <summary>
    /// Method that resets the Game Data (on the Load)
    /// </summary>
    /// <param name="objectsToBeLoaded"></param>
    /// <returns></returns>
    public static void LoadData() {
        if (ins != null) {
            ins.LoadGameData();
        }

    }

    /// <summary>
    /// Method that Loads the game (full-restart)
    /// </summary>
    public static void Load() {
        if (ins != null) {
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
        if (ins != null) {
            return ins.playerProfile.currentScene;
        }
        return -1;
    }

    public static void SavePlayerSettings() {
        if(ins != null) {
            ins.SaveGameSettings();
        }
    }
}
