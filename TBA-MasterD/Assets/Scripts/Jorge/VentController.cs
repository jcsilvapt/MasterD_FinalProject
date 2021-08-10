using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentController : MonoBehaviour, IDamage {

    [SerializeField] GameObject unbrokenVent;
    [SerializeField] GameObject brokenVent;



    private bool hasBeenDestroyed = false;

    public void TakeDamage() {
        if (!hasBeenDestroyed) {
            unbrokenVent.SetActive(false);
            brokenVent.SetActive(true);
            SetDelayToDestroy(brokenVent);
            hasBeenDestroyed = !hasBeenDestroyed;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!hasBeenDestroyed) {
            if (other.tag == "DroneDart" || other.tag == "Bullet") {
                unbrokenVent.SetActive(false);
                brokenVent.SetActive(true);
                SetDelayToDestroy(brokenVent);
                hasBeenDestroyed = !hasBeenDestroyed;
                Destroy(other.gameObject);
            }
        }
    }



    private void SetDelayToDestroy(GameObject obj) {
        Transform temp = obj.GetComponent<Transform>();
        foreach (Transform b in temp) {
            Destroy(b.gameObject, Random.Range(5f, 15f));
        }
    }

}
