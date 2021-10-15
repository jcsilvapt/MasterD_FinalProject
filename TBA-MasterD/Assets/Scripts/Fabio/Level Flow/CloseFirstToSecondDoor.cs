using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseFirstToSecondDoor : MonoBehaviour
{
    [SerializeField] private SecondLevelManager secondLevelManager;

    #region 

    private bool hasBeenActivated;

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !hasBeenActivated)
        {
            //VERIFICA SE JÁ PASSOU NA OUTRA SALA!
            //if(other.GetComponent<charController>()){
            //
            //}
            secondLevelManager.LockDoorFirstToSecondLevel();
            secondLevelManager.SetNormalAudio();
            hasBeenActivated = true;
        }
    }
}
