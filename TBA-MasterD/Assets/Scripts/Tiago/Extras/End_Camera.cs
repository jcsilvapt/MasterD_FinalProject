using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End_Camera : MonoBehaviour
{
    [SerializeField] GameObject escapeCar;
    void Update()
    {
        this.transform.LookAt(escapeCar.transform.position);
    }
}
