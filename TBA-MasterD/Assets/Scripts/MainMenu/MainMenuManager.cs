using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour {

    [Header("UI - Main")]
    [SerializeField] GameObject Menu;
    [SerializeField] GameObject Loading;
    [SerializeField] GameObject Credits;
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera secondaryCamera;

    [Header("UI - Navigation Main")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject playMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject creditsMenu;

    [Header("UI - Play Game")]
    [SerializeField] Button btnContinue;

    [Header("UI - Navigation Options")]
    [SerializeField] GameObject videoPanel;
    [SerializeField] GameObject audioPanel;
    [SerializeField] GameObject controlsPanel;

    [Header("UI - Save System")]
    [SerializeField] GameObject saveInfoPanel;


    [Header("Developer")]
    [SerializeField] GameObject devCurrentPanelSelected;
    [SerializeField] GameObject devCurrentMainPanelSelected;
    [SerializeField] string devCurrentResolutionSelected;
    [SerializeField] string devCurrentDisplayModeSelected;
    [SerializeField] bool isCredits = false;

    public List<GameObject> initial = new List<GameObject>();

    public List<GameObject> menus = new List<GameObject>();

    private bool hasSave = false;

    // Start is called before the first frame update
    void Start() {

        GameManager.SetCursorVisibility(true);

        saveInfoPanel.SetActive(false);

        initial.Add(Menu);
        initial.Add(Loading);

        menus.Add(mainMenu);
        menus.Add(playMenu);
        menus.Add(optionsMenu);
        menus.Add(videoPanel);
        menus.Add(audioPanel);
        menus.Add(controlsPanel);
        menus.Add(creditsMenu);

        SetAllAlphasToZero();

        devCurrentPanelSelected = mainMenu;
        devCurrentMainPanelSelected = Menu; 

        btnContinue.interactable = false;

    }

    private void Update() {
        
        if (hasSave) return;

        if(SaveSystemManager.HasSavedData()) {
            hasSave = true;
            btnContinue.interactable = true;
        }
    }

    private void SetAllAlphasToZero() {

        foreach (GameObject a in menus) {
            a.SetActive(true);
            a.GetComponent<CanvasGroup>().alpha = 0;
            a.GetComponent<CanvasGroup>().interactable = false;
            a.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        mainMenu.GetComponent<CanvasGroup>().alpha = 1;
        mainMenu.GetComponent<CanvasGroup>().interactable = true;
        mainMenu.GetComponent<CanvasGroup>().blocksRaycasts = true;

        foreach(GameObject a in initial) {
            a.SetActive(true);
            a.GetComponent<CanvasGroup>().alpha = 0;
            a.GetComponent<CanvasGroup>().interactable = false;
            a.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        Menu.GetComponent<CanvasGroup>().alpha = 1;
        Menu.GetComponent<CanvasGroup>().interactable = true;
        Menu.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }


    public void btn_swapMenu(string menu) {
        FadeEffect(devCurrentPanelSelected, FadeType.OUT);

        switch (menu) {
            case "play":
                devCurrentPanelSelected = playMenu;
                break;
            case "options":
                devCurrentPanelSelected = optionsMenu;
                break;
            case "credits":
                devCurrentPanelSelected = creditsMenu;
                SetCreditsPanel(true);
                break;
            case "menu":
                devCurrentPanelSelected = mainMenu;
                if(isCredits) {
                    SetCreditsPanel(false);
                }
                break;
            case "options_video":
                devCurrentPanelSelected = videoPanel;
                break;
            case "options_audio":
                devCurrentPanelSelected = audioPanel;
                break;
            case "options_controls":
                devCurrentPanelSelected = controlsPanel;
                break;
        }

        FadeEffect(devCurrentPanelSelected, FadeType.IN);
    }

    /// <summary>
    /// Function that fades the Object
    /// </summary>
    /// <param name="current">Must have Canvas Group Attached</param>
    /// <param name="FadeType">"I" Fade In | "Out" Fade Out</param>
    private void FadeEffect(GameObject current, FadeType FadeType) {

        // Get Canvas group out of the GameObject
        CanvasGroup cg = current.GetComponent<CanvasGroup>();

        switch (FadeType) {
            case FadeType.IN:
                StartCoroutine(FadeIn(cg));
                break;
            case FadeType.OUT:
                StartCoroutine(FadeOut(cg));
                break;
        }
    }

    #region Public Buttons

    public void btnQuit() {
        GameManager.QuitGame();
    }

    public void btnLoadNewGame() {
        if (SaveSystemManager.HasSavedData()) {
            saveInfoPanel.SetActive(true);
        } else {
            GameManager.ChangeScene(1, false);
        }
    }

    public void btnForceNewGame() {
        GameManager.ChangeScene(1, false);
    }

    public void btnContinueGame() {
        GameManager.ChangeScene(SaveSystemManager.GetCurrentSaveScene(), true);
    }

    private void SetCreditsPanel(bool value) {
        StartCoroutine(CreditsLoad(Menu, Credits, mainCamera, secondaryCamera, value));
    }

    public void btnCancel() {
        saveInfoPanel.SetActive(false);
    }

    #endregion


    #region COROUTINES
    public bool isFading = false;

    IEnumerator FadeIn(CanvasGroup cg) {

        while (cg.alpha < 1) {
            if (!isFading) {
                cg.alpha += Time.deltaTime * 3f;
            }
            yield return null;
        }

        cg.interactable = true;
        cg.blocksRaycasts = true;
        yield return null;

    }

    IEnumerator FadeOut(CanvasGroup cg) {
        isFading = true;
        while (cg.alpha > 0) {
            cg.alpha -= Time.deltaTime * 15f;
            yield return null;
        }
        cg.interactable = false;
        cg.blocksRaycasts = false;
        isFading = false;
        yield return null;
    }

    IEnumerator CreditsLoad(GameObject Menu, GameObject Credits, Camera mainCamera, Camera secondaryCamera, bool turnCreditsOn) {
        CanvasGroup cgCredits = Credits.GetComponent<CanvasGroup>();
        while(cgCredits.alpha < 1) {
            cgCredits.alpha += Time.deltaTime * 3f;
            yield return null;
        }

        isCredits = turnCreditsOn;

        mainCamera.enabled = !turnCreditsOn;
        secondaryCamera.enabled = turnCreditsOn;

        while(cgCredits.alpha > 0) {
            cgCredits.alpha -= Time.deltaTime * 1f;
            yield return null;
        }
        yield return null;
    }

    #endregion
}

public enum FadeType {
    IN,
    OUT
}