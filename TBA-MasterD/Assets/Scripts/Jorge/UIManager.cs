using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager ins;

    public GameObject uiCrosshair;
    public GameObject uiInteraction;

    private void Awake() {
        if(ins == null) {
            ins = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
        }
    }

    private void ToggleCrosshair(bool value) {
        if(uiCrosshair == null && uiInteraction == null) {
            uiCrosshair = GameObject.Find("Crosshair");
            uiInteraction = GameObject.Find("Interaction");
        }
        uiCrosshair.SetActive(value);
        uiInteraction.SetActive(!value);
    }

    public static void UI_ToggleCrosshair(bool value) {
        if(ins != null) {
            ins.ToggleCrosshair(value);
        }
    }

}
