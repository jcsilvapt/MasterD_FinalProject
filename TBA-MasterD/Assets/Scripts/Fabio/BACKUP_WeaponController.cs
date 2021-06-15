using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BACKUP_WeaponController : MonoBehaviour
{
    /* SO Save System
     * - SO_Jogador
     * - SO_Armas
     * - SO_Inimigos
     * 
     * - Save System deve ser Singleton para existir em todo o lado.
     * A qualquer altura, fazer, SaveSystem.Save;
     * 
     * - Checkpoints;
     */

    #region References

    //Weapons that Can Be Utilized
    [SerializeField] private Weapon[] weaponsAll;

    //Weapons the Player has With it
    [SerializeField] private Weapon[] weaponsPossessed;

    #endregion

    //Index Holding Current Weapon
    private int currentWeaponIndex;

    //Current Weapon Equipped
    private Weapon currentWeaponEquipped;

    //Time Between Mouse Wheel Movements
    [SerializeField] private float timeBetweenMouseWheelMovement;

    //Timer Keeping the Mouse Wheel Movements in Check
    private float timeCheckBetweenMouseWheelMovements;

    //Flag Holding if the player is switching Weapons
    private bool isChangingWeapon;

    #region Weapon Stats

    public ShootingType[] shootingTypes;
    public int shootingTypeIndex;
    public int maximumBullets;
    public int clipSize;
    public int bulletsInClip;
    public float timeBetweenShots;
    public float timeReload;

    #endregion

    private void Start()
    {
        currentWeaponIndex = 0;
        currentWeaponEquipped = weaponsPossessed[currentWeaponIndex];

        SetWeaponStats();

        isChangingWeapon = false;
    }

    private void Update()
    {
        ChangeCurrentWeapon(Input.GetAxis("Mouse ScrollWheel"));
        
        Shooting();

        Reload();
        
        Debug.Log(currentWeaponEquipped.name);
    }

    private void ChangeCurrentWeapon(float indexIndicator)
    {
        //If TimeCheck still isn't 0, Decrement Time Check Mouse Wheel Movements and return.
        //Changing Weapon Flag is now true
        if(timeCheckBetweenMouseWheelMovements > 0)
        {
            isChangingWeapon = true;
            timeCheckBetweenMouseWheelMovements -= Time.deltaTime;
            return;
        }

        //Changing Weapon Flag is now true
        isChangingWeapon = false;

        //If Mouse Wheel didn't move or Weapons Possessed only has one weapon, return.
        if (indexIndicator == 0 || weaponsPossessed.Length <= 1)
        {
            return;
        }
        
        //IF -> Mouse Wheel Rotated Upwards
        //ELSE -> Mouse Wheel Rotated Downwards
        if (indexIndicator > 0)
        {
            //IF -> the Current Weapon Index is in the final slot of the array and it's incremented, this goes back to the beginning of the array
            //ELSE -> the Current Weapon Index is simply incremented to the next slot of the array
            if (currentWeaponIndex >= weaponsPossessed.Length - 1)
            {
                currentWeaponIndex = 0;
            }
            else
            {
                currentWeaponIndex++;
            }
        }
        else
        {
            //IF -> the Current Weapon Index is in the first slot of the array and it's decremented, this goes to the end of the array
            //ELSE -> the Current Weapon Index is simply decremented to the previous slot of the array
            if (currentWeaponIndex <= 0)
            {
                currentWeaponIndex = weaponsPossessed.Length - 1;
            }
            else
            {
                currentWeaponIndex--;
            }
        }

        //Set Current Weapon Stats
        currentWeaponEquipped.SetAllStats(shootingTypeIndex, maximumBullets, bulletsInClip);

        //Change Current Weapon Equipped based on the changes made to the Current Weapon Index
        currentWeaponEquipped = weaponsPossessed[currentWeaponIndex];

        //Update Weapon Stats
        SetWeaponStats();

        //Set Time Check Mouse Wheel Movements
        timeCheckBetweenMouseWheelMovements = timeBetweenMouseWheelMovement;
    }

    private void Shooting()
    {
        //If the Player's Changing Weapon, Cannot shoot, return.
        if (isChangingWeapon)
        {
            return;
        }

        //If the player doesn't have Bullets in Clip, return.
        if(bulletsInClip <= 0)
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
        //If Player didn't Press the Reload Key, return.
        if (!Input.GetKeyDown(KeyCode.R))
        {
            return;
        }

        //If the Weapon's Clip is full, return.
        if(bulletsInClip == clipSize)
        {
            return;
        }

        //If the Weapon doesn't have any Bullets Available, return.
        if(maximumBullets <= 0)
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

    private void SetWeaponStats()
    {
        //Get All the Weapon Stats
        List<string> weaponStats = currentWeaponEquipped.GetAllStats();

        //Get Ammount of Shooting Types
        int shootingTypeRange = int.Parse(weaponStats[0]);

        //Reset Shooting Types array based on Weapon Shooting Type Size
        shootingTypes = new ShootingType[shootingTypeRange];

        //Fill the Shooting Type Array
        for (int shootingTypeIndex = 1; shootingTypeIndex <= shootingTypeRange; shootingTypeIndex++)
        {
            switch (weaponStats[shootingTypeIndex])
            {
                case "Automatic":
                    shootingTypes[shootingTypeIndex - 1] = ShootingType.Automatic;
                    break;
                case "Burst":
                    shootingTypes[shootingTypeIndex - 1] = ShootingType.Burst;
                    break;
                case "Single":
                    shootingTypes[shootingTypeIndex - 1] = ShootingType.Single;
                    break;
            }
        }

        //Get All the Other Weapon Stats
        shootingTypeIndex = int.Parse(weaponStats[++shootingTypeRange]);
        maximumBullets = int.Parse(weaponStats[++shootingTypeRange]);
        clipSize = int.Parse(weaponStats[++shootingTypeRange]);
        bulletsInClip = int.Parse(weaponStats[++shootingTypeRange]);
        timeBetweenShots = float.Parse(weaponStats[++shootingTypeRange]);
        timeReload = int.Parse(weaponStats[++shootingTypeRange]);
    }

    /* TODO:
     * Controla tudo relacionado com as armas
     * Mudar o Shooting Type
     * Input's
     * Disparo
     */

}
