using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Collider))]
public class Modal : MonoBehaviour {

    [Header("Modal Data")]
    [SerializeField] string modalTitle = "tutorial";
    [TextArea(5, 10)]
    [SerializeField] string modalText;

    [Header("Modal Settings")]
    [Tooltip("Select which tag will trigger this event")]
    [SerializeField] TargetTag targetTag = TargetTag.PLAYER;
    [Tooltip("Set true if you wish to ignore the trigger so another method can activate the modal")]
    [SerializeField] bool ignoreTrigger = false;
    [Tooltip("Set true if you wish this modal opens everytime the player triggers")]
    [SerializeField] bool isMultiple = false;

    [Header("Developer")]
    [SerializeField] CanvasGroup cg;
    [SerializeField] TMP_Text tmpTitle;
    [SerializeField] TMP_Text tmpText;
    [SerializeField] bool isActivated = false;
    [SerializeField] string temp = "";
    [SerializeField] bool fadingOccurring = false;

    private void Start() {
        tmpTitle.text = modalTitle;
        tmpText.text = modalText;

        if (cg.alpha != 0) {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    #region TRIGGER

    private void OnTriggerEnter(Collider other) {
        if (!ignoreTrigger) {
            if (targetTag == TargetTag.PLAYER) {
                if (other.CompareTag("Player") || other.CompareTag("PlayerParent")) {
                    ShowModal();
                }
            } else {
                if (other.CompareTag("Drone")) {
                    ShowModal();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player") || other.CompareTag("PlayerParent") || other.CompareTag("Drone")) {
            HideModal();
        }
    }

    #endregion

    #region PUBLIC

    public void ShowModal() {
        if (!isActivated) {
            StartCoroutine(FadeIn());
        }
    }

    public void HideModal() {
        if (isMultiple) {
            isActivated = false;
            if (ignoreTrigger) {
                ignoreTrigger = false;
            }
        }
        StartCoroutine(FadeOut());
    }
    #endregion

    #region COROUTINES
    IEnumerator FadeIn() {
        if (!fadingOccurring) {
            fadingOccurring = true;
            while (cg.alpha < 1) {
                cg.alpha += Time.unscaledDeltaTime * 3f;
                yield return null;
            }
            isActivated = true;
            fadingOccurring = false;
        }

        yield return new WaitForSeconds(5f);

        StartCoroutine(FadeOut());

    }

    IEnumerator FadeOut() {
        if (!fadingOccurring) {
            fadingOccurring = true;
            while (cg.alpha > 0) {
                cg.alpha -= Time.unscaledDeltaTime * 15f;
                yield return null;
            }
            fadingOccurring = false;
        }
    }
    #endregion
}

public enum TargetTag {
    PLAYER,
    DRONE
}