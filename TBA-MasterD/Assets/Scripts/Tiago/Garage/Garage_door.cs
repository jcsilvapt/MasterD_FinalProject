using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garage_door : MonoBehaviour
{
    private Animator anim;
    [SerializeField] charController player;
    [SerializeField] GameObject stealthObjects;
    
        // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }  

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (player.isStealth == false)
            {
                stealthObjects.SetActive(true);
            }
           
            anim.SetBool("isOpen",true);
        }
    }
}
