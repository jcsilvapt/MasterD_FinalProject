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
        }
    }

    private void ToggleCrosshair(bool value) {
        uiCrosshair.SetActive(value);
        uiInteraction.SetActive(!value);
    }

    public static void UI_ToggleCrosshair(bool value) {
        if(ins != null) {
            ins.ToggleCrosshair(value);
        }
    }

}
