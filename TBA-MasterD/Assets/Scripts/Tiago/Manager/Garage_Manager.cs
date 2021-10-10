using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garage_Manager : MonoBehaviour
{
    [Header("Barrier")]

    public GameObject barrier;
    private Animator animBarr;

    [Header("Music")]
    [SerializeField] GameObject stealthMusic;
    [SerializeField] GameObject actionMusic;

    [Header("Player")]
    [SerializeField] charController player;
    private float mapBeginnerHealth;
    public bool isStealth = true;

    [Header("EndGame Enemies")]
    [SerializeField] GameObject enemyHolder;
    [SerializeField] GameObject endGameEnemies;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerParent").GetComponent<charController>();
        animBarr = barrier.GetComponent<Animator>();
        stealthMusic.SetActive(true);
        actionMusic.SetActive(false);
        mapBeginnerHealth = player.GetHealth();
    }
    private void Update()
    {
        if (isStealth == false)
        {
            stealthMusic.SetActive(false);
            actionMusic.SetActive(true);
        }

        if (player.GetHealth() < mapBeginnerHealth)
        {
            isStealth = false;
        }
    }

    public void GoToMainMenu() {
        GameManager.ChangeScene(0, false);
    }

    #region Barrier Rotation
    public void RotateBarrier()
    {
        animBarr.SetTrigger("isRotate");
    }

    #endregion   

    #region End Scene
    public void EndingEnemies()
    {
        enemyHolder.SetActive(false);
        endGameEnemies.SetActive(true);
    }

    #endregion
}
