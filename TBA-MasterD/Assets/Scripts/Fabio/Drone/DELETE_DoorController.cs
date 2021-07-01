using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DELETE_DoorController : MonoBehaviour
{
    //Target Door Reference
    [SerializeField] private GameObject[] targetDoors;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "DroneDart")
        {
            foreach(GameObject door in targetDoors)
            {
                door.SetActive(!door.activeSelf);
            }
        }
    }
}
