using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenuManager : MonoBehaviour
{
    public void GoToMainMenuButton()
    {
        GameManager.SetPlayerDeath(false);
        GameManager.ChangeScene(0, false);
    }

    public void RestartLastCheckpoint()
    {
        GameManager.SetPlayerDeath(false);
        //GameObject.FindGameObjectWithTag("Player").GetComponent<charController>().GetLastCheckpoint().LoadLastCheckPoint();
        SaveSystemManager.Load();
    }
}