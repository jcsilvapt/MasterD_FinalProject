using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier_Button : MonoBehaviour
{
    public Garage_Manager gm;

    private void Start()
    {
       // gm = GetComponent<Garage_Manager>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DroneDart")
        {
            Debug.Log("Got Hit");
            gm.RotateBarrier();
        }
    }
}
