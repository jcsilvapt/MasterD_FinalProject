using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier_Button : MonoBehaviour, IDamage
{
    public Garage_Manager gm;   
   
    public void TakeDamage()
    {
        gm.RotateBarrier();
    }
}
