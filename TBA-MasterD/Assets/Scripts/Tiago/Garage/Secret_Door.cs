using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Secret_Door : MonoBehaviour
{
    private Animator anim;
    public GameObject door;

    private void Start()
    {
        anim = door.GetComponent<Animator>();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            anim.SetBool("gotOpen", true);
        }
    }
}
