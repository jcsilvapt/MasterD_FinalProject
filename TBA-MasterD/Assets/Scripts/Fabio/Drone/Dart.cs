using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    //Rigidbody Reference
    private Rigidbody rb;

    //Speed the Dart Moves
    [SerializeField] private float dartSpeed;

    void Start()
    {
        //Get Reference
        rb = GetComponent<Rigidbody>();

        Destroy(gameObject, 10f);
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * dartSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
