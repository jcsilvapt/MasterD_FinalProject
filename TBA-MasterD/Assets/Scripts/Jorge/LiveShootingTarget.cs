using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveShootingTarget : MonoBehaviour, IDamage {

    private Animator targetAnim;
    private bool targetIsUp = false;
    public bool hasBeenHit = false;

    private void Start() {
        targetAnim = GetComponent<Animator>();
    }

    /// <summary>
    /// Function that rises the target so that can be shooted.
    /// </summary>
    public void TargetRise() {
        targetAnim.SetBool("GoUp", true);
        targetIsUp = true;
    }

    public void TargetDown() {
        targetAnim.SetBool("GoUp", false);
        targetIsUp = false;
    }

    /// <summary>
    /// Function that returns if the target has been hit already or not.
    /// </summary>
    /// <returns></returns>
    public bool HasBeenHit() {
        return hasBeenHit;
    }

    public void TakeDamage() {
        if(targetIsUp && !hasBeenHit) {
            TargetDown();
            hasBeenHit = true;
        }
    }
}
