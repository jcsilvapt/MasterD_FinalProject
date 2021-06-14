using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponInformation : ScriptableObject
{
    //Shooting Types
    public ShootingType[] shootingType;

    //Maximum Bullets the Player can Carry
    public int maximumBullets;
    
    //Maximum Bullets that can fit in the Clip
    public int clipSize;

    //Current Bullets in the Clip
    public int bulletsInClip;

    //Time it takes between Shots
    public float timeBetweenShots;

    //Time it takes to Reload
    public float timeReload;
}

[System.Serializable]
public enum ShootingType
{
    Single,
    Automatic,
    Burst
}