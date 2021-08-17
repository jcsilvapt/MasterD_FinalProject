using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
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
    public bool canShoot;
    private int numberOfBurstShots;

    #endregion

    [SerializeField] GameObject bulletParticleSystem;
    [SerializeField] GameObject hitEffect;
    [SerializeField] Transform shootingFrom;
    [SerializeField] Animator weaponAnimator;
    [SerializeField] Animator armsAnimator;
    [Header("Decals")]
    [SerializeField] GameObject[] defaultBulletHoles;
    [SerializeField] GameObject[] glassBulletHoles;
    [SerializeField] GameObject[] woodBulletHoles;

    [SerializeField] bool isSpraying;
    [SerializeField] float xSpray;
    [SerializeField] float ySpray;
    [SerializeField] Vector3 spray;

    [SerializeField] Camera cam;

    private void Awake() {
        shootingType = SO_WeaponInformation.shootingType;
        shootingTypeIndex = SO_WeaponInformation.shootingTypeIndex;
        maximumBullets = SO_WeaponInformation.maximumBullets;
        clipSize = SO_WeaponInformation.clipSize;
        bulletsInClip = SO_WeaponInformation.bulletsInClip;
        timeBetweenShots = SO_WeaponInformation.timeBetweenShots;
        timeReload = SO_WeaponInformation.timeReload;

        //anim = GetComponent<Animator>();

        isWeaponActive = false;
        canShoot = true;

        cam = Camera.main;
    }

    public string GetWeaponName() {
        return SO_WeaponInformation.name;
    }

    public void WeaponUpdate() {
        /*
        FireRate();

        Shooting();

        Reload();

        BurstShot();
        */

        // Jorge Testes para depois juntar se necessário

        if (isWeaponActive) {

            if (Input.GetMouseButton(0)) {
                JorgeShooting();
                CalculateSpray();
            }
            if (Input.GetMouseButtonUp(0)) {
                reShot = true;
                armsAnimator.SetBool("isShooting", false);
                isSpraying = false;
            }
            if (Input.GetKeyDown(KeyCode.R)) Reload();

            FireRate();
        }
    }

    public bool reShot = true;

    private void JorgeShooting() {

        if (!HasBulletsInClip()) return;

        switch (shootingType[shootingTypeIndex]) {
            case ShootingType.Single:
                if (canShoot && reShot) {
                    JShoot();
                    reShot = false;
                }
                break;
            case ShootingType.Burst:
                if (canShoot && !isBursting && reShot) {
                    StartCoroutine(BurstShooting());
                    isBursting = true;
                    reShot = false;
                }
                break;
            case ShootingType.Automatic:
                if (canShoot) {
                    JShoot();
                }
                break;
        }
    }

    private IEnumerator BurstShooting() {
        for (int i = 0; i < 3; i++) {
            JShoot();
            yield return new WaitForSeconds(timeBetweenShots);
        }
        isBursting = false;
    }

    private void CalculateSpray() {

        spray = new Vector3(Camera.main.transform.forward.x + Random.Range(-xSpray, xSpray),
                        Camera.main.transform.forward.y + Random.Range(-ySpray, ySpray),
                        Camera.main.transform.forward.z
                        );

    }

    private void JShoot() {

        bulletParticleSystem.SetActive(true); // Enable Particle System

        weaponAnimator.SetBool("isShooting", true);
        armsAnimator.SetBool("isShooting", true);
        armsAnimator.SetTrigger("shoot");
        //anim.SetTrigger("Shoot");

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, spray, out hit)) {

            // On Hit instantiate Particle Effects 'On Hit' and Destroys after 1 second or so...
            GameObject tempHit = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(tempHit, 1f);

            // Calculate in which direction the bullet has hit and then recalculate a fix value so the decal won't appear "flickering" with other textures
            // Probably use something around 0.001f to fix the flickering
            Vector3 decalNewPosition = new Vector3((hit.point.x + hit.normal.x / 1000), (hit.point.y + hit.normal.y / 1000), (hit.point.z + hit.normal.z / 1000));

            // Now checks if the thing that we hit is a destructable or not (doesn't make sense creating a decal on a object that will be changed...)
            if (hit.transform.GetComponent<IDamage>() != null) {
                hit.transform.GetComponent<IDamage>().TakeDamage();
                bulletsInClip--;
                timeElapsedSinceShot = timeBetweenShots;
                canShoot = false;
                return;
            }

            GameObject randomDecal;

            // Ok Seems I will hit something "harder" so let's see what am I hitting...
            switch (hit.transform.tag) {
                case "Glass":
                    randomDecal = glassBulletHoles[Random.Range(0, glassBulletHoles.Length)];
                    break;
                default:
                    randomDecal = defaultBulletHoles[Random.Range(0, defaultBulletHoles.Length)];
                    break;
            }
            // Instantiate decal on the spot
            GameObject temporaryDecal = Instantiate(randomDecal, decalNewPosition, Quaternion.LookRotation(hit.normal));
            temporaryDecal.transform.parent = hit.transform;
            Destroy(temporaryDecal, 3f);
        }


        bulletsInClip--;
        timeElapsedSinceShot = timeBetweenShots;
        canShoot = false;

    }

    private bool HasBulletsInClip() {
        if (bulletsInClip <= 0) {
            armsAnimator.SetBool("isShooting", false);
            return false;
        }
        return true;
    }

    private void BurstShot() {
        //If the Player isn't Bursting, return.
        if (!isBursting) {
            return;
        }

        //If the Player can't Shot, return.
        if (!canShoot) {
            return;
        }

        //If there are no bullets in the Clip, Player stops Bursting, the Number of BurstShots resets and returns.
        if (bulletsInClip <= 0) {
            numberOfBurstShots = 0;
            isBursting = false;
            return;
        }

        //Increments the number of shots and removes one bullet from the clip, Time Since the Last Shot is Updated and, consequently, CanShoot as well.
        numberOfBurstShots++;
        bulletsInClip--;
        timeElapsedSinceShot = timeBetweenShots;

        //If the Player has Bursted three shots, The Number of Burst Shots resets and is no longer Bursting.
        if (numberOfBurstShots == 3) {
            numberOfBurstShots = 0;
            isBursting = false;
        }
    }

    #region Weapon Methods

    private void Shooting() {
        //If the Player's Changing Weapon, Cannot shoot, return.
        if (!isWeaponActive) {
            return;
        }

        //If the player doesn't have Bullets in Clip, return.
        if (bulletsInClip <= 0) {
            return;
        }

        //If the Player is Bursting, return.
        if (isBursting) {
            return;
        }

        //If the player Can't shoot, return.
        if (!canShoot) {
            return;
        }


        //Depending on the current Shooting Type, different Effects and Input's will take place
        switch (shootingType[shootingTypeIndex]) {
            case ShootingType.Automatic:
                if (Input.GetMouseButton(0)) {
                    bulletsInClip--;
                    timeElapsedSinceShot = timeBetweenShots;

                }
                break;

            case ShootingType.Burst:
                if (Input.GetMouseButtonDown(0)) {
                    isBursting = true;
                }
                break;

            case ShootingType.Single:
                if (Input.GetMouseButtonDown(0)) {
                    bulletsInClip--;
                    GameObject tempBullet = Instantiate(bulletParticleSystem, shootingFrom);
                    tempBullet.transform.localPosition = new Vector3(0, 0, 0);
                    tempBullet.transform.parent = null;
                }
                break;
        }

    }

    private void Reload() {
        //If that Weapon isn't active, return.
        if (!isWeaponActive) {
            return;
        }

        //If Player didn't Press the Reload Key, return.
        if (!Input.GetKeyDown(KeyCode.R)) {
            return;
        }

        //If the Weapon's Clip is full, return.
        if (bulletsInClip == clipSize) {
            return;
        }

        //If the Weapon doesn't have any Bullets Available, return.
        if (maximumBullets <= 0) {
            return;
        }

        //IF -> The Number of Bullets to be placed in Bullets In Clip is less than the Number of Maximum Bullets,
        //removing that number from Maximum Bullets and Place it in Bullets In Clip 
        //ELSE -> The Number of Bullets to be placed in Bullets In Clip is more than the Number of Maximum Bullets,
        //Setting Maximum Bullets to 0, and Place them all in Bullets In Clip 
        if (maximumBullets >= clipSize - bulletsInClip) {
            maximumBullets -= (clipSize - bulletsInClip);
            bulletsInClip = clipSize;
        } else {
            bulletsInClip += maximumBullets;
            maximumBullets = 0;
        }

    }

    private void FireRate() {
        //If the Time Since the Last Shot isn't enough, decrement it, update flag so it can't shoot and return.
        if (timeElapsedSinceShot > 0) {
            timeElapsedSinceShot -= Time.deltaTime;
            canShoot = false;
            return;
        }

        //Enough Time has passed since the Last Shot, update Flag.
        canShoot = true;
        bulletParticleSystem.SetActive(false);
        weaponAnimator.SetBool("isShooting", false);
    }

    #endregion

    #region Set's

    public void SetActiveWeapon(bool isChangingWeapon) {
        isWeaponActive = isChangingWeapon;
    }

    #endregion
}
