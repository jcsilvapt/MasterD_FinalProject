using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [Header("Audio")]
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

    private void Start() {


        Debug.LogWarning("Game Manager: To Hide the mouse cursor just press 'K'");
        
        /*
        if (!displayCursor) {
            ToggleCursorVisibility();
        }*/

        // Sets the volume to default values
        mixer.SetFloat("Master", -10);
        mixer.SetFloat("Music", 0);
        mixer.SetFloat("Effects", 0);
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


    }


    #region Public References 

    public static void SetPause() {
        if (ins != null) {
            ins._SetPauseGame();
        }
    }

    public static void SetCursorVisibility() {
        if (ins != null) {
            ins.ToggleCursorVisibility();
        }
    }

    public static bool GetCursorVisibility() {
        if (ins != null) {
            return ins.showCursor;
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

#endregion

private void _SetPauseGame() {
    if (isGamePaused) {
        Time.timeScale = 1;
        isGamePaused = false;
        Debug.Log("Game Manager: Game Unpaused");
    } else {
        Time.timeScale = 0;
        isGamePaused = true;
        Debug.Log("Game Manager: Game Paused");
    }
}

private void ToggleCursorVisibility() {
    showCursor = !showCursor;
    Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Confined;
    Cursor.visible = showCursor ? true : false;
}


}
