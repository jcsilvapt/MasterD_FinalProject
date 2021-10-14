using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointStealthChecker : MonoBehaviour
{
    Garage_Manager gm;
    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject)
        {
            gm = GameObject.FindGameObjectWithTag("Manager").GetComponent<Garage_Manager>();
            gm.StealthOrNot();
        }
        
    }
}



