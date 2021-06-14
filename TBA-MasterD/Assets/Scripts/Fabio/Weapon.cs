using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //Reference SO_Weapon
    [SerializeField] private WeaponInformation SO_WeaponInformation;

    #region Get's

    public List<string> GetAllStats()
    {
        List<string> allStats = new List<string>();

        for(int index = 0; index < SO_WeaponInformation.shootingType.Length; index++)
        {
            allStats.Add(SO_WeaponInformation.shootingType[index].ToString());
        }

        allStats.Add(SO_WeaponInformation.maximumBullets.ToString());
        allStats.Add(SO_WeaponInformation.clipSize.ToString());
        allStats.Add(SO_WeaponInformation.bulletsInClip.ToString());
        allStats.Add(SO_WeaponInformation.timeBetweenShots.ToString());
        allStats.Add(SO_WeaponInformation.timeReload.ToString());

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
}
