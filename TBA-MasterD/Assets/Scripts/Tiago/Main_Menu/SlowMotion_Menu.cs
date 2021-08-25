using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotion_Menu : MonoBehaviour
{
    private bool first = true;
    public Rigidbody[] rb;
    public float timeScaler;
    
    void Start()
    {
        rb = GetComponentsInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        SlowDowntime();
    }

    public void SlowDowntime()
    {
        foreach (Rigidbody child in rb)
        {
            float timer = Time.fixedDeltaTime * timeScaler;

            child.velocity += Physics.gravity / child.mass * timer;
        }
    }
   
  
}
