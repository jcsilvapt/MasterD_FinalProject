using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DELETE_PlayerController : MonoBehaviour
{
    void Update()
    {
        transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * Time.deltaTime;
    }
}
