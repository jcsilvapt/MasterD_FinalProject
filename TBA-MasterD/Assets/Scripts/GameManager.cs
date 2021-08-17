using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager ins;

    #region SINGLETON INITIALIZATION

    private void Awake() {
        if (ins == null) {
            ins = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
        }
    }
    #endregion

    #region DECLARATIONS

    [SerializeField] bool isGamePaused = false;
    [SerializeField] bool showCursor = true;

    #endregion

    #region DEVELOPER SETTINGS

    [SerializeField] bool displayCursor = false;

    #endregion

    private void Start() {
        Debug.LogWarning("Game Manager: To Hide the mouse cursor just press 'K'");

        if(!displayCursor) {
            ToggleCursorVisibility();
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
