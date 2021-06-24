using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class casingScript : MonoBehaviour
{
    public float timeToDie;
    void Start()
    {
        StartCoroutine(selfDestruct());
    }

    
    IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(timeToDie);
        Destroy(gameObject);
    }
}
