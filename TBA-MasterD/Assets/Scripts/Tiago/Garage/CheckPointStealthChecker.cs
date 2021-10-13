using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointStealthChecker : MonoBehaviour
{

    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "PlayerParent")
        {
            PlayerPrefs.GetInt("Stealh");
            return;
        }
    }
}


