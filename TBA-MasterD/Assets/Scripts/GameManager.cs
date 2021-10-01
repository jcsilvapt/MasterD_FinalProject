using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour {

    public static GameManager ins;

    #region SINGLETON INITIALIZATION

    private void Awake() {
        if (ins == null) {
            ins = this;
            DontDestroyOnLoad(this);
            GetInitialData();
        } else {
            Destroy(gameObject);
        }
    }
    #endregion

    #region DECLARATIONS

    [Header("Audio Settings")]
    [SerializeField] AudioMixer mixer;

    [SerializeField] bool isGamePaused = false;
    [SerializeField] bool showCursor = true;

    #endregion

    #region VIDEO SETTINGS

    [SerializeField] List<string> displayModes = new List<string>() { "Fullscreen", "Window" };
    [SerializeField] List<string> resolutions = new List<string>();
    [SerializeField] List<string> quality = new List<string>() { "Very Low", "Low", "Medium", "High", "Very High", "Ultra" };

    [SerializeField] bool isFullscreen;
    [SerializeField] Resolution currentResolution;
    [SerializeField] int currentQualitySelected;

    #endregion

    #region DEVELOPER SETTINGS

    [SerializeField] bool displayCursor = false;

    #endregion

    #region Load System
    [Header("Load System UI")]
    [SerializeField] GameObject loadCanvas;
    [SerializeField] Slider loadSlider;
    #endregion

    #region Game Data
    [Header("Game Data Settings")]
    [SerializeField] SO_PlayerData playerProfile;
    private SaveSystem saveSystem;
    #endregion

    private void Start() {

        // Sets the volume to default values
        mixer.SetFloat("Master", -10);
        mixer.SetFloat("Music", 0);
        mixer.SetFloat("Effects", 0);

        // Set inactive load screen
        loadCanvas.SetActive(false);
    }

    private void GetInitialData() {
        // Set current quality
        currentQualitySelected = quality.Count - 1;

        // Set current Resolution
        currentResolution = Screen.currentResolution;

        // Set fullscreen
        isFullscreen = Screen.fullScreen;

        // Get all availables Screen Resolutions
        foreach (Resolution r in Screen.resolutions) {
            resolutions.Add(r.ToString());
        }

        // Initialize Save System
        saveSystem = new SaveSystem();



    }

    #region Public References 

    public static void SetPause() {
        if (ins != null) {
            ins._SetPauseGame();
        }
    }

    public static bool GetPause()
    {
        if (ins != null)
        {
            return ins.isGamePaused;
        }

        return false;
    }

    public static void SetCursorVisibility(bool showCursor) {
        if (ins != null) {
            ins.ToggleCursorVisibility(showCursor);
        }
    }

    public static bool GetCursorVisibility() {
        if (ins != null) {
            return ins.isGamePaused;
        }
        return false;
    }

    public static List<string> GetDisplayModes() {
        if (ins != null) {
            return ins.displayModes;
        }
        return null;
    }

    public static List<string> GetAvailableResolutions() {
        if (ins != null) {
            return ins.resolutions;
        }
        return null;
    }

    public static Resolution GetCurrentResolution() {
        if (ins != null) {
            return Screen.currentResolution;
        }
        return Screen.currentResolution;
    }

    public static bool GetFullscreen() {
        if (ins != null) {
            return ins.isFullscreen;
        }
        return false;
    }

    public static int GetQualityLevel() {
        if (ins != null) {
            return QualitySettings.GetQualityLevel();
        }
        return -1;
    }

    public static void QuitGame() {
        if (ins != null) {
            Application.Quit();
        }
    }

    public static void SetMasterVolume(float volume) {
        if (ins != null) {
            ins.mixer.SetFloat("Master", volume);
        }
    }

    public static void SetMusicVolume(float volume) {
        if (ins != null) {
            ins.mixer.SetFloat("Music", volume);
        }
    }

    public static void SetEffectVolume(float volume) {
        if (ins != null) {
            ins.mixer.SetFloat("Effects", volume);
        }
    }

    public static float GetMasterVolume() {
        if (ins != null) {
            float value;
            if (ins.mixer.GetFloat("Master", out value)) {
                return value;
            }
            return value;
        }
        return -80;
    }

    public static float GetMusicVolume() {
        if (ins != null) {
            float value;
            if (ins.mixer.GetFloat("Music", out value)) {
                return value;
            }
            return value;
        }
        return -80;
    }

    public static float GetEffectVolume() {
        if (ins != null) {
            float value;
            if (ins.mixer.GetFloat("Effects", out value)) {
                return value;
            }
            return value;
        }
        return -80;
    }

    public static void ChangeScene(int sceneIndex, bool showLoadPanel, bool loadGameData) {
        if (ins != null) {
            ins._changeScene(sceneIndex, showLoadPanel, loadGameData);
        }
    }

    public static void ChangeScene(string sceneName, bool loadGameData, bool loadAdditive) {
        if(ins != null) {
            ins._changeScene(sceneName, loadGameData, loadAdditive);
        }
    }

    public static void SaveGame() {
        if (ins != null) {
            ins.Save();
        }
    }

    public static void LoadGame() {
        if (ins != null) {
            ins.Load();
        }
    }

    public static bool HasDataSaved() {
        if(ins != null) {
            return ins.saveSystem.HasSavedData();
        }
        return false;
    }

    #endregion

    #region Logic

    private void Save() {
        Debug.Log(Application.persistentDataPath); 
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerProfile.currentPosition = player.transform.position;
        playerProfile.currentScene = SceneManager.GetActiveScene().buildIndex;
        saveSystem.Save(playerProfile);

        Debug.Log("Saving Game...");
    }

    private void Load() {
        playerProfile = saveSystem.Load();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = playerProfile.currentPosition;

        Debug.Log("Loading Game...");
    }

    private void _SetPauseGame() {
        if (isGamePaused) {
            ToggleCursorVisibility(true);
            Time.timeScale = 0;
            isGamePaused = true;
            Debug.Log("Game Manager: Game Paused");
        } else {
            ToggleCursorVisibility(false);
            Time.timeScale = 1;
            isGamePaused = false;
            Debug.Log("Game Manager: Game Unpaused");
        }
    }

    private void ToggleCursorVisibility(bool showCursor) {
        this.showCursor = showCursor;
        Cursor.lockState = this.showCursor ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = this.showCursor ? true : false;
    }

    private void _changeScene(int sceneIndex, bool showLoadScreen, bool loadGameData) {
        if (showLoadScreen) {
            loadCanvas.SetActive(true);
            StartCoroutine(LoadAsync(sceneIndex, loadGameData));
        } else {
            SceneManager.LoadScene(sceneIndex);
        }
    }

    private void _changeScene(string sceneName, bool loadGameData, bool loadAdditive) {
        if(loadAdditive) {
            Debug.LogWarning("GAME MANAGER LOADING::::::::::::::::");
            StartCoroutine(LoadAsync(sceneName));
        }
    }

    #endregion

    #region COROUTINES

    /// <summary>
    /// Coroutine that loads a scene (default scene loader)
    /// </summary>
    /// <param name="sceneIndex"></param>
    /// <param name="loadGameData"></param>
    /// <returns></returns>
    IEnumerator LoadAsync(int sceneIndex, bool loadGameData) {

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            loadSlider.value = progress;

            yield return null;
        }

        loadSlider.value = 0;
        loadCanvas.SetActive(false);

        if (loadGameData)
            GameManager.LoadGame();
    }

    /// <summary>
    /// Coroutine that loads a scene by Name (used to Additive Mode)
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    IEnumerator LoadAsync(string sceneName) {
        Debug.LogWarning("LOAD ASYNC:::::::::::::::::::");
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!operation.isDone) {
            yield return null;
        }
    }

    #endregion
}