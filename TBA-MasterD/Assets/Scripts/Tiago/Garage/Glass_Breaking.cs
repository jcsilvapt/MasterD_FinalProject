using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass_Breaking : MonoBehaviour,IDamage
{
    public Garage_Manager gm;

    public void TakeDamage()
    {
        gm.switchGlass();
    }

   
}
