using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuAudio : MonoBehaviour {

    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider effectsSlider;

    private void Start() {
        masterSlider.value = -5;
        musicSlider.value = 0;
        effectsSlider.value = 0;
    }

    public void MasterVolume(float value) {
        GameManager.SetMasterVolume(value);
    }
    public void MusicVolume(float value) {
        GameManager.SetMusicVolume(value);
    }
    public void EffectsVolume(float value) {
        GameManager.SetEffectVolume(value);
    }

}
