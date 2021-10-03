using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garage_Manager : MonoBehaviour
{
    [Header("Barrier")]

    public GameObject barrier;
    private Animator animBarr;

    [Header("Glass")]
    public GameObject normalGlass;
    public GameObject brokenGlass;

    [Header("Music")]
    [SerializeField] GameObject stealthMusic;
    [SerializeField] GameObject actionMusic;

    [Header("Player")]
    [SerializeField] charController player;
    private float mapBeginnerHealth;
    public bool isStealth = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<charController>();
        animBarr = barrier.GetComponent<Animator>();
        stealthMusic.SetActive(true);
        actionMusic.SetActive(false);
        mapBeginnerHealth = player.GetHealth();
    }
    private void Update()
    {
     if(isStealth == false)
        {
            stealthMusic.SetActive(false);
            actionMusic.SetActive(true);
        }

     if(player.GetHealth() < mapBeginnerHealth)
        {
            isStealth = false;
        }
    }
    #region Barrier Rotation
    public void RotateBarrier()
    {
        animBarr.SetTrigger("isRotate");
    }

    #endregion

    #region Breaking Glass
    public void switchGlass()
    {
        normalGlass.SetActive(false);
        brokenGlass.SetActive(true);
    }

    #endregion

    #region Music
    
    #endregion
}
