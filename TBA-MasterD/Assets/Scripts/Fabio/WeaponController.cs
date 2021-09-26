using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
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
    [Tooltip("Weapons that can be utilized")]
    [SerializeField] private Weapon[] weaponsAll;

    //Weapons the Player has With it
    [Tooltip("Weapons the player has with it")]
    [SerializeField] private Weapon[] weaponsPossessed;

    #endregion

    #region Current Weapon and Change Weapon Variables

    //Index Holding Current Weapon
    private int currentWeaponIndex;

    //Current Weapon Equipped
    private Weapon currentWeaponEquipped;

    //Time Between Mouse Wheel Movements
    [SerializeField] private float timeBetweenMouseWheelMovement;

    //Timer Keeping the Mouse Wheel Movements in Check
    private float timeCheckBetweenMouseWheelMovements;

    #endregion

    #region JORGE

    [SerializeField] bool hasWeapon;
    [SerializeField] Animator animator;
    [SerializeField] bool AKOn;
    [SerializeField] bool PistolOn;

    #endregion


    private void Awake() {

        // On Enable this script the weapons should be disabled (since the character starts without weapons)

        foreach (Weapon weapon in weaponsPossessed) {
            weapon.gameObject.SetActive(false);
        }

        /*
        if (hasWeapon) {

            currentWeaponIndex = 0;
            currentWeaponEquipped = weaponsPossessed[currentWeaponIndex];

            currentWeaponEquipped.gameObject.SetActive(true);

            if (currentWeaponEquipped.GetWeaponName().Contains("AK-47")) {
                animator.SetBool("AK", true);
            } else {
                animator.SetBool("Pistol", true);
            }
            currentWeaponEquipped.SetActiveWeapon(true);
        }*/
    }


    private void Update() {

        if (currentWeaponEquipped == null) return;

        currentWeaponEquipped.WeaponUpdate();

        ChangeCurrentWeapon(Input.GetAxis("Mouse ScrollWheel"));


    }

    private void ChangeCurrentWeapon(float indexIndicator) {
        //If TimeCheck still isn't 0, Decrement Time Check Mouse Wheel Movements and return.
        //Changing Weapon Flag is now true
        if (timeCheckBetweenMouseWheelMovements > 0) {
            timeCheckBetweenMouseWheelMovements -= Time.deltaTime;
            return;
        }

        //If Mouse Wheel didn't move or Weapons Possessed only has one weapon, return.
        if (indexIndicator == 0 || weaponsPossessed.Length <= 1) {
            return;
        }

        //IF -> Mouse Wheel Rotated Upwards
        //ELSE -> Mouse Wheel Rotated Downwards
        if (indexIndicator > 0) {
            //IF -> the Current Weapon Index is in the final slot of the array and it's incremented, this goes back to the beginning of the array
            //ELSE -> the Current Weapon Index is simply incremented to the next slot of the array
            if (currentWeaponIndex >= weaponsPossessed.Length - 1) {
                currentWeaponIndex = 0;
            } else {
                currentWeaponIndex++;
            }
        } else {
            //IF -> the Current Weapon Index is in the first slot of the array and it's decremented, this goes to the end of the array
            //ELSE -> the Current Weapon Index is simply decremented to the previous slot of the array
            if (currentWeaponIndex <= 0) {
                currentWeaponIndex = weaponsPossessed.Length - 1;
            } else {
                currentWeaponIndex--;
            }
        }

        //Set Current Weapon Stats
        currentWeaponEquipped.SetActiveWeapon(false);
        if (currentWeaponEquipped.GetWeaponName().Contains("AK-47")) {
            animator.SetBool("AK", false);
        } else {
            animator.SetBool("Pistol", false);
        }
        currentWeaponEquipped.gameObject.SetActive(false);

        //Change Current Weapon Equipped based on the changes made to the Current Weapon Index
        currentWeaponEquipped = weaponsPossessed[currentWeaponIndex];
        currentWeaponEquipped.SetActiveWeapon(true);
        if (currentWeaponEquipped.GetWeaponName().Contains("AK-47")) {
            animator.SetBool("AK", true);
        } else {
            animator.SetBool("Pistol", true);
        }
        currentWeaponEquipped.gameObject.SetActive(true);

        //Set Time Check Mouse Wheel Movements
        timeCheckBetweenMouseWheelMovements = timeBetweenMouseWheelMovement;
    }

    /* TODO:
     * Controla tudo relacionado com as armas
     * Mudar o Shooting Type
     * Input's
     * Disparo
     */

    #region JORGE

    /// <summary>
    /// Function that will enable the weapon display and controls
    /// </summary>
    public void EnableWeapon() {
        hasWeapon = true;

        currentWeaponIndex = 0;
        currentWeaponEquipped = weaponsPossessed[currentWeaponIndex];

        currentWeaponEquipped.gameObject.SetActive(true);

        if (currentWeaponEquipped.GetWeaponName().Contains("AK-47")) {
            animator.SetBool("AK", true);
        } else {
            animator.SetBool("Pistol", true);
        }
        currentWeaponEquipped.SetActiveWeapon(true);
    }

    public void DisableCurrentWeapon() {
        currentWeaponEquipped.gameObject.SetActive(false);
        currentWeaponEquipped = null;
    }

    public bool HasWeapon() {
        return hasWeapon;
    }

    /// <summary>
    /// X: Maximum Bullets | Y: Current Bullets
    /// </summary>
    /// <returns></returns>
    public Vector2 GetCurrentWeaponAmmo() {
        return currentWeaponEquipped.GetWeaponBullets();
    }

    public void AddBulletsToCurrentWeapon(int amount) {
        currentWeaponEquipped.AddBullets(amount);
    }

    #endregion

    #region Fabio Edit

    public Weapon[] GetPlayerWeapons()
    {
        return weaponsPossessed;
    }

    #endregion
}
