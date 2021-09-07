using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class btnSettings : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [TextArea(1,2)]
    [SerializeField] string buttonDetails;

    [SerializeField] TMP_Text onScreenDetailsText;

    [Header("Developer Settings")]
    [SerializeField] string defaultScreenText = "Choose one of the options";

    private void Start() {
        onScreenDetailsText.text = defaultScreenText;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        onScreenDetailsText.text = buttonDetails;
    }

    public void OnPointerExit(PointerEventData eventData) {
        onScreenDetailsText.text = defaultScreenText;
    }
}
