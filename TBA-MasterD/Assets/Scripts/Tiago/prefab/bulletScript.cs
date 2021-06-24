using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    private Rigidbody rb;
    public float moveSpeed;
    public float timeToDie;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(selfDestruct());
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

    IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(timeToDie);
        Destroy(gameObject);
    }
}
