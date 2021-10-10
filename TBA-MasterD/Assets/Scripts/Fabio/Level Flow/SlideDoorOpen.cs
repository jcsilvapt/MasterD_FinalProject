using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideDoorOpen : MonoBehaviour
{
    #region References

    //Animator Reference
    private Animator animator;

    //List Holding Enemies
    [SerializeField] private List<Fabio_EnemySecondLevel> enemiesList;

    //Subtitle System
    private Subtitles subtitleSystem;
    [SerializeField] private AudioClip voiceLine;
    [SerializeField] private string subtitles;

    #endregion

    #region Control Variables

    private bool isActive;

    #endregion

    private void Start()
    {
        animator = GetComponent<Animator>();
        subtitleSystem = GetComponent<Subtitles>();

        isActive = true;
    }

    private void Update()
    {
        if (isActive)
        {
            for (int enemy = 0; enemy < enemiesList.Count; enemy++)
            {
                if (enemiesList[enemy].GetEnemyHealth() <= 0)
                {
                    enemiesList.RemoveAt(enemy);
                }
            }

            if(enemiesList.Count == 0)
            {
                isActive = false;
                OpenSlideDoor();
            }
        }
    }

    private void OpenSlideDoor()
    {
        subtitleSystem.SetAudioAndSubtitles(voiceLine, subtitles, null);

        animator.SetBool("Open", true);
        animator.SetBool("side", true);
    }
}
