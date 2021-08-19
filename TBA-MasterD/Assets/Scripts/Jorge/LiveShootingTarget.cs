using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveShootingTarget : MonoBehaviour, IDamage {

    public event Action beenShoot; 

    private Animator targetAnim;
    private bool targetIsUp = false;
    private bool hasBeenHit = false;

    private void Start() {
        targetAnim = GetComponent<Animator>();
    }

    /// <summary>
    /// Function that rises the target so that can be shooted.
    /// </summary>
    public void TargetRise() {
        targetAnim.SetBool("GoUp", true);
    }

    /// <summary>
    /// Function that returns if the target has been hit already or not.
    /// </summary>
    /// <returns></returns>
    public bool HasBeenHit() {
        return hasBeenHit;
    }

    public void TakeDamage() {
        hasBeenHit = true;
        targetAnim.SetBool("GoUp", false);
    }


}
