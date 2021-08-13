using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class secret_door_stay : MonoBehaviour
{
    private Animator anim;
    public GameObject door;
    void Start()
    {
        anim = door.GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            anim.SetBool("gotOpen", false);
        }
    }
}
