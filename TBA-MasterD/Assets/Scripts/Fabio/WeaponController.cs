using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    //Weapons that Can Be Utilized
    [SerializeField] private Weapon[] weaponsAll;

    //Weapons the Player has With it
    [SerializeField] private Weapon[] weaponsPossessed;

    //Index Holding Current Weapon
    private int currentWeaponIndex;

    //Current Weapon Equipped
    private Weapon currentWeaponEquipped;

    private void Start()
    {
        currentWeaponIndex = 0;
        currentWeaponEquipped = weaponsPossessed[currentWeaponIndex];
    }

    private void Update()
    {
        Debug.Log(Input.GetAxis("Mouse ScrollWheel"));

        //Change Mouse Scroll Wheel (Add a Timer that locks the movement)
        ChangeCurrentWeapon(Input.GetAxis("Mouse ScrollWheel"));

        /*
         * Ou seja, quando mexer a roda do rato, um timer começa a contar e, até esse timer chegar ao fim, todo o movimento
         * feito pela roda do rato é descartado (meio segundo ou algo do género).
         * Quando esse timer chegar ao fim, todo o movimento é novamente registado e repete.
         */
    }

    private void ChangeCurrentWeapon(float indexIndicator)
    {
        //If Mouse Wheel didn't move or Weapons Possessed only has one weapon, return.
        if(indexIndicator == 0 || weaponsPossessed.Length <= 1)
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

        //Change Current Weapon Equipped based on the changes made to the Current Weapon Index
        currentWeaponEquipped = weaponsPossessed[currentWeaponIndex];
    }

    //TODO:
    /* Controla tudo relacionado com as armas
     * Mudar o Shooting Type
     * Input's
     * Disparo
     * Reload's
     */

}
