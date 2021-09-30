using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManagerActivationControlRoom : MonoBehaviour
{
    //AI Manager Reference
    [SerializeField] private Fabio_AIManager aiManager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            aiManager.PlayerDetected();
            gameObject.SetActive(false);
        }
    }
}
