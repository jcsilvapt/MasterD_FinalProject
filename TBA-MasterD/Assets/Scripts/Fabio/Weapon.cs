using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Reference SO_Weapon
    
    [SerializeField] private WeaponInformation SO_WeaponInformation;
    
    #endregion

    #region Local Weapon Stats
    [SerializeField] private ShootingType[] shootingType;
    [SerializeField] private int shootingTypeIndex;
    [SerializeField] private int maximumBullets;
    [SerializeField] private int clipSize;
    [SerializeField] private int bulletsInClip;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float timeReload;

    [SerializeField] private float timeElapsedSinceShot;
    [SerializeField] private bool isWeaponActive;
    private bool isBursting;
    private bool canShoot;
    private int numberOfBurstShots;

    #endregion

    private void Awake()
    {
        shootingType = SO_WeaponInformation.shootingType;
        shootingTypeIndex = SO_WeaponInformation.shootingTypeIndex;
        maximumBullets = SO_WeaponInformation.maximumBullets;
        clipSize = SO_WeaponInformation.clipSize;
        bulletsInClip = SO_WeaponInformation.bulletsInClip;
        timeBetweenShots = SO_WeaponInformation.timeBetweenShots;
        timeReload = SO_WeaponInformation.timeReload;

        isWeaponActive = false;
        canShoot = true;
    }

    public void WeaponUpdate()
    {
        FireRate();

        Shooting();

        Reload();

        BurstShot();
    }

    private void BurstShot()
    {
        //If the Player isn't Bursting, return.
        if (!isBursting)
        {
            return;
        }

        //If the Player can't Shot, return.
        if (!canShoot)
        {
            return;
        }

        //If there are no bullets in the Clip, Player stops Bursting, the Number of BurstShots resets and returns.
        if(bulletsInClip <= 0)
        {
            numberOfBurstShots = 0;
            isBursting = false;
            return;
        }

        //Increments the number of shots and removes one bullet from the clip, Time Since the Last Shot is Updated and, consequently, CanShoot as well.
        numberOfBurstShots++;
        bulletsInClip--;
        timeElapsedSinceShot = timeBetweenShots;

        //If the Player has Bursted three shots, The Number of Burst Shots resets and is no longer Bursting.
        if(numberOfBurstShots == 3)
        {
            numberOfBurstShots = 0;
            isBursting = false;
        }
    }

    #region Weapon Methods

    private void Shooting()
    {
        //If the Player's Changing Weapon, Cannot shoot, return.
        if (!isWeaponActive)
        {
            return;
        }

        //If the player doesn't have Bullets in Clip, return.
        if (bulletsInClip <= 0)
        {
            return;
        }
        
        //If the Player is Bursting, return.
        if (isBursting)
        {
            return;
        }

        //If the player Can't shoot, return.
        if (!canShoot)
        {
            return;
        }


        //Depending on the current Shooting Type, different Effects and Input's will take place
        switch (shootingType[shootingTypeIndex])
        {
            case ShootingType.Automatic:
                if (Input.GetMouseButton(0))
                {
                    bulletsInClip--;
                    timeElapsedSinceShot = timeBetweenShots;
                }
                break;

            case ShootingType.Burst:
                if (Input.GetMouseButtonDown(0))
                {
                    isBursting = true;
                }
                break;

            case ShootingType.Single:
                if (Input.GetMouseButtonDown(0))
                {
                    bulletsInClip--;
                }
                break;
        }
    }

    private void Reload()
    {
        //If that Weapon isn't active, return.
        if (!isWeaponActive)
        {
            return;
        }

        //If Player didn't Press the Reload Key, return.
        if (!Input.GetKeyDown(KeyCode.R))
        {
            return;
        }

        //If the Weapon's Clip is full, return.
        if (bulletsInClip == clipSize)
        {
            return;
        }

        //If the Weapon doesn't have any Bullets Available, return.
        if (maximumBullets <= 0)
        {
            return;
        }

        //IF -> The Number of Bullets to be placed in Bullets In Clip is less than the Number of Maximum Bullets,
        //removing that number from Maximum Bullets and Place it in Bullets In Clip 
        //ELSE -> The Number of Bullets to be placed in Bullets In Clip is more than the Number of Maximum Bullets,
        //Setting Maximum Bullets to 0, and Place them all in Bullets In Clip 
        if (maximumBullets >= clipSize - bulletsInClip)
        {
            maximumBullets -= (clipSize - bulletsInClip);
            bulletsInClip = clipSize;
        }
        else
        {
            bulletsInClip += maximumBullets;
            maximumBullets = 0;
        }

    }

    private void FireRate()
    {
        //If the Time Since the Last Shot isn't enough, decrement it, update flag so it can't shoot and return.
        if (timeElapsedSinceShot > 0)
        {
            timeElapsedSinceShot -= Time.deltaTime;
            canShoot = false;
            return;
        }

        //Enough Time has passed since the Last Shot, update Flag.
        canShoot = true;
    }

    #endregion

    #region Set's

    public void SetActiveWeapon(bool isChangingWeapon)
    {
        isWeaponActive = isChangingWeapon;
    }

    #endregion
}
