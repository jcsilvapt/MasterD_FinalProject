using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Use this class when you want that the object  that have this script will be parent of something.
/// 
/// Example: Crane box moving, when the player set foot on this object, the player gameObject will be moved to this zone.
/// </summary>
[RequireComponent(typeof(Collider))]
public class ParentSwitcher : MonoBehaviour {

    [Header("Settings")]
    [Tooltip("Object to be search for (Default: Player)")]
    [SerializeField] string objectTag = "Player";

    [Header("Developer Settings")]
    [Tooltip("This shows only the object that will be attached to this gameobject (Parent)")]
    [SerializeField] Transform _Object = null;
    [SerializeField] bool hasObject = false;


    private void OnTriggerEnter(Collider other) {
        if (!hasObject) {
            if (other.CompareTag(objectTag)) {
                _Object = GameObject.Find(objectTag).transform;
                _Object.parent = this.transform;
                hasObject = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag(objectTag)) {
            _Object.parent = null;
            _Object = null;
            hasObject = false;
        }
    }
}
