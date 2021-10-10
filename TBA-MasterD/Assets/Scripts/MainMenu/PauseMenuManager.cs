using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI - Pause Menu")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenu;

    [Header("UI - PAuse Options")]
    [SerializeField] GameObject videoPanel;
    [SerializeField] GameObject audioPanel;
    [SerializeField] GameObject controlsPanel;

    [Header("UI - Quit Option")]
    [SerializeField] GameObject quitPanel;


    [Header("Developer")]
    [SerializeField] GameObject devCurrentPanelSelected;
    [SerializeField] string devCurrentResolutionSelected;
    [SerializeField] string devCurrentDisplayModeSelected;

    public List<GameObject> menus = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        quitPanel.SetActive(false);

        menus.Add(pauseMenu);
        menus.Add(optionsMenu);
        menus.Add(videoPanel);
        menus.Add(audioPanel);
        menus.Add(controlsPanel);

        SetAllAlphasToZero();

        devCurrentPanelSelected = pauseMenu;
    }

    private void SetAllAlphasToZero()
    {

        foreach (GameObject a in menus)
        {
            a.SetActive(true);
            a.GetComponent<CanvasGroup>().alpha = 0;
            a.GetComponent<CanvasGroup>().interactable = false;
            a.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        pauseMenu.GetComponent<CanvasGroup>().alpha = 1;
        pauseMenu.GetComponent<CanvasGroup>().interactable = true;
        pauseMenu.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }


    public void btn_swapMenu(string menu)
    {
        FadeEffect(devCurrentPanelSelected, FadeType.OUT);

        switch (menu)
        {
            case "pause":
                devCurrentPanelSelected = pauseMenu;
                break;
            case "options":
                devCurrentPanelSelected = optionsMenu;
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
    private void FadeEffect(GameObject current, FadeType FadeType)
    {

        // Get Canvas group out of the GameObject
        CanvasGroup cg = current.GetComponent<CanvasGroup>();

        switch (FadeType)
        {
            case FadeType.IN:
                StartCoroutine(FadeIn(cg));
                break;
            case FadeType.OUT:
                StartCoroutine(FadeOut(cg));
                break;
        }

    }

    #region Public Buttons

    public void btnResume()
    {
        GameManager.SetPause();
    }

    public void btnRestartCheckpoint()
    {
        SaveSystemManager.Load();
    }

    public void btnQuit()
    {
        quitPanel.SetActive(false);
        pauseMenu.SetActive(false);
        GameManager.SetPause();
        GameManager.ChangeScene(0, false);
    }

    public void btnQuitGame()
    {
        quitPanel.SetActive(true);
    }

    public void btnCancel()
    {
        quitPanel.SetActive(false);
    }

    #endregion


    #region COROUTINES
    public bool isFading = false;

    IEnumerator FadeIn(CanvasGroup cg)
    {
        while (cg.alpha < 1)
        {
            if (!isFading)
            {
                cg.alpha += Time.unscaledDeltaTime * 3f;
            }
            yield return null;
        }

        cg.interactable = true;
        cg.blocksRaycasts = true;
        yield return null;

    }

    IEnumerator FadeOut(CanvasGroup cg)
    {
        isFading = true;
        while (cg.alpha > 0)
        {
            cg.alpha -= Time.unscaledDeltaTime * 5f;
            yield return null;
        }
        cg.interactable = false;
        cg.blocksRaycasts = false;
        isFading = false;
        yield return null;
    }
    #endregion
}
