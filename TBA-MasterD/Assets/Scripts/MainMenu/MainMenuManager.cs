using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour {

    [Header("UI - Navigation Main")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject playMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject creditsMenu;

    [Header("UI - Navigation Options")]
    [SerializeField] GameObject videoPanel;
    [SerializeField] GameObject audioPanel;
    [SerializeField] GameObject controlsPanel;


    [Header("Developer")]
    [SerializeField] GameObject devCurrentPanelSelected;
    [SerializeField] string devCurrentResolutionSelected;
    [SerializeField] string devCurrentDisplayModeSelected;

    public List<GameObject> menus = new List<GameObject>();

    // Start is called before the first frame update
    void Start() {
        menus.Add(mainMenu);
        menus.Add(playMenu);
        menus.Add(optionsMenu);
        menus.Add(videoPanel);
        menus.Add(audioPanel);
        menus.Add(controlsPanel);

        SetAllAlphasToZero();

        devCurrentPanelSelected = mainMenu;
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
                break;
            case "menu":
                devCurrentPanelSelected = mainMenu;
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

    public void btnQuit() {
        GameManager.QuitGame();
    }

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

}

public enum FadeType {
    IN,
    OUT
}