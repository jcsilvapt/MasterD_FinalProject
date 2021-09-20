using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropsScript : MonoBehaviour
{
    [SerializeField] charController player;
    [SerializeField] Weapon pistol;
    [SerializeField] Weapon ak;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckWichPackDrops();
    }

    private void CheckWichPackDrops()
    {
        if (player.health <= 50)
        {
            Debug.Log("Dropped health");
        }
        else if (pistol.maximumBullets <= pistol.maximumBullets / 2)
        {
            Debug.Log("Dropped Ammo");
        }
        else if (ak.maximumBullets <= 60)
        {
            Debug.Log("Dropped Ammo");
        }
    }
}
