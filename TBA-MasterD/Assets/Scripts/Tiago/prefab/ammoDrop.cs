using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoDrop : MonoBehaviour
{
    [SerializeField] Weapon pistol;
    [SerializeField] Weapon ak;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            /* pistol.maximumBullets = pistol.maximumBullets + 10;
             ak.maximumBullets = pistol.maximumBullets + 0;*/
            Destroy(this);
        }
    }
}
