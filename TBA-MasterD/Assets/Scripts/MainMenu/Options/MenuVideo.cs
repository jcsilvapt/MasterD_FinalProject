using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuVideo : MonoBehaviour {

    // Referencia aos tipos de resoluções
    [SerializeField] List<string> resolutions = new List<string>();
    [SerializeField] List<string> quality = new List<string>() { "Very Low", "Low", "Medium", "High", "Very High", "Ultra" };


    [Header("References")]
    [SerializeField] TMP_Text displayText;
    [SerializeField] TMP_Text resolutionText;
    [SerializeField] TMP_Text qualityText;
    [SerializeField] GameObject applyPanel;

    [Header("Settings - HAS WAS")]
    [SerializeField] string currentResolution;
    [SerializeField] int currentResolutionId;
    [SerializeField] bool isFullScreen;
    [SerializeField] int currentQualitySettingId;

    [Header("Settings - RunTime")]
    [SerializeField] string r_Resolution;
    [SerializeField] int r_ResolutionId;
    [SerializeField] bool r_isFullscreen;
    [SerializeField] int r_QualitySettingsId;

    [Header("Developer")]
    [SerializeField] bool hasValuesChanged = false;

    private void Start() {
        // Set initial video settings
        SetInitialVideoSettings();
        // Resets all runtime variables
        ResetRuntimeVariables();

        // Update aos valores da ui
        UpdateUI();
    }

    /// <summary>
    /// Function that gets the current video settings.
    /// Also applies this settings to the runtime variables so we can check what has been change
    /// to show the apply button (or not).
    /// </summary>
    private void SetInitialVideoSettings() {
        // Get Current Application settings
        resolutions = GameManager.GetAvailableResolutions();
        currentResolution = GameManager.GetCurrentResolution().ToString();
        isFullScreen = GameManager.GetFullscreen();
        currentQualitySettingId = GameManager.GetQualityLevel();
        // Set the current ID of the resolution being used.
        for (int i = 0; i < resolutions.Count; i++) {
            if (currentResolution.Equals(resolutions[i])) {
                currentResolutionId = i;
                break;
            }
        }
    }

    /// <summary>
    /// Function that resets all runtime variables so they can match the previous values
    /// </summary>
    private void ResetRuntimeVariables() {
        // Reset all runtime Variables
        r_Resolution = currentResolution;
        r_isFullscreen = isFullScreen;
        r_QualitySettingsId = currentQualitySettingId;
        r_ResolutionId = currentResolutionId;
    }

    /// <summary>
    /// Although is not perfectly optimize, this function checks all conditions if they've changed.
    /// </summary>
    /// <returns>
    /// True there are changes.
    /// false there aren't any changes.
    /// </returns>
    private bool CheckChanges() {
        //Check if the resolution has been changed
        if (r_ResolutionId != currentResolutionId) return true;
        // Check if the fullscreen mode has been changed
        if (r_isFullscreen != isFullScreen) return true;
        // Check if the quality settings has been changed
        if (r_QualitySettingsId != currentQualitySettingId) return true;
        // If every above check is false than there is no changes
        return false;
    }

    private void UpdateUI() {
        displayText.text = isFullScreen ? "Fullscreen" : "Windowed";
        resolutionText.text = currentResolution;
        qualityText.text = quality[currentQualitySettingId];
        applyPanel.SetActive(CheckChanges());
    }

    public void btnChangeResolution(int n) {
        if (n > 0) {
            if (currentResolutionId + n > resolutions.Count - 1) {
                currentResolutionId = 0;
            } else {
                currentResolutionId += n;
            }
        } else {
            if (currentResolutionId + n < 0) {
                currentResolutionId = resolutions.Count - 1;
            } else {
                currentResolutionId += n;
            }
        }
        currentResolution = resolutions[currentResolutionId];
        hasValuesChanged = true;
        UpdateUI();
    }

    public void btnChangeDisplay() {
        isFullScreen = !isFullScreen;
        hasValuesChanged = true;
        UpdateUI();
    }

    public void btnChangeQuality(int n) {
        if (n > 0) {
            if (currentQualitySettingId + n > quality.Count - 1) {
                currentQualitySettingId = 0;
            } else {
                currentQualitySettingId += n;
            }
        } else {
            if (currentQualitySettingId + n < 0) {
                currentQualitySettingId = quality.Count - 1;
            } else {
                currentQualitySettingId += n;
            }
        }
        hasValuesChanged = true;
        UpdateUI();
    }

    public void btnApplyChanges() {
        // Call GameManager and apply the settings
        // GameManager.Set....

        // Reset all the runtime variables so the apply button disapears
        ResetRuntimeVariables();
        UpdateUI();
    }
}