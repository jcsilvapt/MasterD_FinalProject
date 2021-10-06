using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseFirstToSecondDoor : MonoBehaviour
{
    [SerializeField] private SecondLevelManager secondLevelManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //VERIFICA SE JÁ PASSOU NA OUTRA SALA!
            //if(other.GetComponent<charController>()){
            //
            //}
            secondLevelManager.LockDoorFirstToSecondLevel();
        }

        Destroy(gameObject);
    }
}
