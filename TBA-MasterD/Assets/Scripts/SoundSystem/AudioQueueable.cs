using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioQueueable : MonoBehaviour {

    [SerializeField] List<AudioClip> audioqueue = new List<AudioClip>();
    [SerializeField] List<AudioClip> audioRestart = new List<AudioClip>();

    [SerializeField] AudioSource earPieceSource;

    [Header("Developer")]
    [SerializeField] AudioClip currentPlaying;


    //private AudioSource earPieceSource;


    private bool isPlaying = false;
    private bool isPlayingImportantSound = false;
    private bool isRestartVoice = false;

    private float timeElapsed = 0;
    private float timePerSentence = 5f;

    private void Start() {
       // earPieceSource = GetComponent<AudioSource>();
    }

    private void Update() {

        if(isPlayingImportantSound) {
            if(!earPieceSource.isPlaying) {
                earPieceSource.Stop();
                earPieceSource.clip = null;
                if (audioqueue.Count > 0) {
                    isRestartVoice = true;
                    currentPlaying = audioRestart[Random.Range(0, audioRestart.Count)];
                    earPieceSource.clip = currentPlaying;
                    earPieceSource.Play();
                }
                isPlayingImportantSound = false;


                while(timeElapsed <= timePerSentence) {
                    timeElapsed += Time.deltaTime;
                }

                timeElapsed = 0;

            }
        } else {
            if(isRestartVoice) {
                if(!earPieceSource.isPlaying) {
                    isRestartVoice = false;
                    earPieceSource.Stop();
                    currentPlaying = null;
                    earPieceSource.clip = null;

                    while (timeElapsed <= timePerSentence) {
                        timeElapsed += Time.deltaTime;
                    }

                    timeElapsed = 0;
                }
            } else {
                if(!isPlaying && audioqueue.Count > 0) {
                    currentPlaying = audioqueue[0];
                    earPieceSource.clip = currentPlaying;
                    earPieceSource.Play();
                    isPlaying = true;
                } else {
                    if(!earPieceSource.isPlaying && audioqueue.Count > 0) {
                        earPieceSource.Stop();
                        earPieceSource.clip = null;
                        currentPlaying = null;
                        audioqueue.RemoveAt(0);
                        isPlaying = false;
                    }
                }
            }
        }



        /*
        if (!isPlayingImportantSound) {
            if (!isPlaying) {
                if(isRestartVoice) {

                }
                if (audioqueue.Count > 0) {
                    currentPlaying = audioqueue[0];
                    aSource.clip = currentPlaying;
                    aSource.Play();
                    isPlaying = true;
                }
            }

            if (!aSource.isPlaying && isPlaying && !isPlayingImportantSound) {
                aSource.Stop();
                aSource.clip = null;
                audioqueue.RemoveAt(0);
                isPlaying = false;
            }
        } else {
            if(!aSource.isPlaying) {
                aSource.Stop();
                aSource.clip = null;
                isPlayingImportantSound = false;
            }
        }*/
    }

    public void SetAudioToQueue(AudioClip clip) {
        audioqueue.Add(clip);
    }

    public void SetImportantAudio(AudioClip clip) {
        isPlayingImportantSound = true;
        earPieceSource.Stop();
        currentPlaying = clip;
        earPieceSource.clip = currentPlaying;
        earPieceSource.Play();
        isPlaying = false;
        isRestartVoice = true;
    }

}
