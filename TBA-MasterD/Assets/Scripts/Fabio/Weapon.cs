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

    [SerializeField] private bool isWeaponActive;

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
    }

    public void WeaponUpdate()
    {
        Shooting();

        Reload();
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

        if (Input.GetMouseButtonDown(0))
        {
            bulletsInClip--;
        }
    }

    private void Reload()
    {
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

    #endregion

    #region Get's

    public List<string> GetAllStats()
    {
        List<string> allStats = new List<string>();

        allStats.Add(shootingType.Length.ToString());

        for(int index = 0; index < shootingType.Length; index++)
        {
            allStats.Add(shootingType[index].ToString());
        }

        allStats.Add(shootingTypeIndex.ToString());
        allStats.Add(maximumBullets.ToString());
        allStats.Add(clipSize.ToString());
        allStats.Add(bulletsInClip.ToString());
        allStats.Add(timeBetweenShots.ToString());
        allStats.Add(timeReload.ToString());

        return allStats;
    }

    public string[] GetShootingTypes()
    {
        string[] shootingTypes = new string[SO_WeaponInformation.shootingType.Length];

        for (int index = 0; index < SO_WeaponInformation.shootingType.Length; index++)
        {
            shootingTypes[index] = SO_WeaponInformation.shootingType[index].ToString();
        }

        return shootingTypes;
    }

    public int GetMaximumBullets()
    {
        return SO_WeaponInformation.maximumBullets;
    }

    public int GetClipSize()
    {
        return SO_WeaponInformation.clipSize;
    }

    public int GetBulletsInClip() 
    {
        return SO_WeaponInformation.bulletsInClip;
    }

    public float GetTimeBetweenShots()
    {
        return SO_WeaponInformation.timeBetweenShots;
    }

    public float GetTimeReload()
    {
        return SO_WeaponInformation.timeReload;
    }

    #endregion

    #region Set's

    public void SetActiveWeapon(bool isChangingWeapon)
    {
        isWeaponActive = isChangingWeapon;
    }

    #endregion
}
