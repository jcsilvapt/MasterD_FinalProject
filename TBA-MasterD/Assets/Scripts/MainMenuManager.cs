using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {


    [Header("Navigation")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject playMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject creditsMenu;

    public GameObject currentMenuSelected;

    public List<GameObject> menus = new List<GameObject>();

    // Start is called before the first frame update
    void Start() {

        menus.Add(mainMenu);
        menus.Add(playMenu);
        menus.Add(optionsMenu);
        //menus.Add(creditsMenu);

        SetAllAlphasToZero();

        currentMenuSelected = mainMenu;
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
        FadeEffect(currentMenuSelected, FadeType.OUT);

        switch (menu) {
            case "play":
                currentMenuSelected = playMenu;
                break;
            case "options":
                currentMenuSelected = optionsMenu;
                break;
            case "credits":
                currentMenuSelected = creditsMenu;
                break;
            case "menu":
                currentMenuSelected = mainMenu;
                break;
        }

        FadeEffect(currentMenuSelected, FadeType.IN);
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

    public bool isFading = false;

    IEnumerator FadeIn(CanvasGroup cg) {
        
        while (cg.alpha < 1) {
            if (!isFading) {
                Debug.Log("FadeIn: While");
                cg.alpha += Time.deltaTime * 3f;
            }
            yield return null;
        }

        Debug.Log("FadeIn: Finish");
        cg.interactable = true;
        cg.blocksRaycasts = true;
        yield return null;

    }

    IEnumerator FadeOut(CanvasGroup cg) {
        isFading = true;
        while (cg.alpha > 0) {
            Debug.Log("FadeOut: While");
            cg.alpha -= Time.deltaTime * 15f;
            yield return null;
        }
        Debug.Log("FadeOut: Finish");
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