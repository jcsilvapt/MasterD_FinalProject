using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DELETE_DetectPlayer : MonoBehaviour
{
    private bool hasDetected;
    [SerializeField] private Transform bich;

    // Start is called before the first frame update
    void Start()
    {
        hasDetected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (AIUtils_Fabio.HasVisionOfPlayer(transform, bich, 4f, LayerMask.NameToLayer("Window"))) {
            hasDetected = true;
        } else {
            hasDetected = false;
        }

        if (hasDetected)
        {
            transform.position = Vector3.MoveTowards(transform.position, bich.position, Time.deltaTime * 5f);
        }
        else
        {
            transform.eulerAngles += new Vector3(0, 10, 0) * Time.deltaTime;
        }
    }
}
