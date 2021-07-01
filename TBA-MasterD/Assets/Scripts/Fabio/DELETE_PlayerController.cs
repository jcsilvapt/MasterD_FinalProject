using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DELETE_PlayerController : MonoBehaviour
{
    [SerializeField] private Transform enemy;

    public float walkSpeed;
    public float runSpeed;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * Time.deltaTime * runSpeed;
            return;
        }

        transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * Time.deltaTime * walkSpeed;
    }
}
