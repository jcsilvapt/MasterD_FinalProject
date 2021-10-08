using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Subtitles : MonoBehaviour {
    [Header("Text Display")]
    [TextArea(1, 5)]
    [Tooltip("Input Text here that will be written")]
    [SerializeField] string subsToWrite;

    [Header("Audio")]
    [SerializeField] AudioClip audioToBePlayed;
    [SerializeField] bool isImportant = false;

    [Header("DEVELOPER TOOLS")]
    [SerializeField] TextMeshProUGUI subtitles;
    [SerializeField] GameObject text;
    private int letterIndex;
    private float timePerLetter;
    private float timer;
    private bool isActive = false;
    private bool hasBeenActive = false;

    private void Awake() {
        Writer(subtitles, subsToWrite, 0.05f); //text variable used on inspector, the text to be written, time it takes per letter
    }

    public void Writer(TextMeshProUGUI subs, string ToWrite, float timerLetter) {
        this.subtitles = subs;
        this.subsToWrite = ToWrite;
        this.timePerLetter = timerLetter;
        letterIndex = 0;
    }

    private void Update() {
        if (isActive == true) {
            StartWriting();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!hasBeenActive) {
            if (other.CompareTag("Player")) {
                if (subsToWrite.Length > 0 && subsToWrite != null) {
                    text.SetActive(true);
                    isActive = true;
                }
                hasBeenActive = !hasBeenActive;
                AudioQueueable queue = other.transform.parent.parent.GetComponent<AudioQueueable>();
                if (isImportant) {
                    queue.SetImportantAudio(audioToBePlayed);
                } else {
                    queue.SetAudioToQueue(audioToBePlayed);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        isActive = false;
        text.SetActive(false);
    }

    private void StartWriting() {
        if (subtitles != null) {
            timer -= Time.deltaTime;
            while (timer <= 0f) //while para evitar que seja afetado por frames
            {
                timer += timePerLetter;
                letterIndex++;
                subtitles.text = subsToWrite.Substring(0, letterIndex);

                if (letterIndex >= subsToWrite.Length) {
                    subtitles = null;
                    return;
                }
            }
        }
    }
}
