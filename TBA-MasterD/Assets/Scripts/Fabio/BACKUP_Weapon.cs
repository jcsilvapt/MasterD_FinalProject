using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BACKUP_Weapon : MonoBehaviour
{
    //Reference SO_Weapon
    [SerializeField] private WeaponInformation SO_WeaponInformation;

    //Local Weapon Stats
    private ShootingType[] shootingType;
    private int shootingTypeIndex;
    private int maximumBullets;
    private int clipSize;
    private int bulletsInClip;
    private float timeBetweenShots;
    private float timeReload;

    private void Awake()
    {
        shootingType = SO_WeaponInformation.shootingType;
        shootingTypeIndex = SO_WeaponInformation.shootingTypeIndex;
        maximumBullets = SO_WeaponInformation.maximumBullets;
        clipSize = SO_WeaponInformation.clipSize;
        bulletsInClip = SO_WeaponInformation.bulletsInClip;
        timeBetweenShots = SO_WeaponInformation.timeBetweenShots;
        timeReload = SO_WeaponInformation.timeReload;
    }

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

    public void SetAllStats(int shootingTypeIndex, int maximumBullets, int bulletsInClip)
    {
        this.shootingTypeIndex = shootingTypeIndex;
        this.maximumBullets = maximumBullets;
        this.bulletsInClip = bulletsInClip;
    }

    #endregion

}
