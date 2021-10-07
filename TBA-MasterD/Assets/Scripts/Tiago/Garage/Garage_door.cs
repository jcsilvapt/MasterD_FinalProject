using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garage_door : MonoBehaviour
{
    private Animator anim;
    [SerializeField] Garage_Manager gm;
    [SerializeField] GameObject stealthObjects;

    void Start()
    {
        anim = GetComponent<Animator>();
    }  

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (gm.isStealth == false)
            {
                stealthObjects.SetActive(true);
            }
           
            anim.SetBool("isOpen",true);
        }
    }
}
