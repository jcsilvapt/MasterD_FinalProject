using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DELETE_Vent : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "DroneDart")
        {
            Destroy(gameObject);
        }
    }
}
