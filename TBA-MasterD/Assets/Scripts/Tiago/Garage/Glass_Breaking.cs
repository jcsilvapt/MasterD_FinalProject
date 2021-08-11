using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass_Breaking : MonoBehaviour
{
    public Garage_Manager gm;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.other.tag == "DroneDart" || collision.other.tag == "Bullet")
        {
            gm.switchGlass();
        }

    }

}
