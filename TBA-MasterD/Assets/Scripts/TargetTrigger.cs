using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour, IDamage {

    public LiveShootingTarget target;

    public void TakeDamage() {
        target.hasBeenHit = true;
    }
}
