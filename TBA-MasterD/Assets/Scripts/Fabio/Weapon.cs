using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Reference SO_Weapon

    [SerializeField] private WeaponInformation SO_WeaponInformation;

    #endregion

    #region Local Weapon Stats
    [SerializeField] private ShootingType[] shootingType;
    [SerializeField] private int shootingTypeIndex;
    public int maximumBullets;
    [SerializeField] private int clipSize;
    [SerializeField] private int bulletsInClip;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float timeReload;

    [SerializeField] private float timeElapsedSinceShot;
    [SerializeField] private bool isWeaponActive;
    private bool isBursting;
    public bool canShoot = true;
    public bool isReloading = false;
    private int numberOfBurstShots;
    public bool reShot = true;

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
    [SerializeField] int bulletsFired = 0;
    [SerializeField] float xSpray;
    [SerializeField] float ySpray;
    [SerializeField] float zSpray;
    [SerializeField] Vector3 spray;

    [SerializeField] Camera cam;

    [SerializeField] AudioSource audioShoot; // by tiago for shooting <--------------------------------------------------------------------------
    [SerializeField] AudioSource audioReload; // by tiago for shooting <--------------------------------------------------------------------------

    private void Awake()
    {
        // Initialize all weapon status
        shootingType = SO_WeaponInformation.shootingType;
        shootingTypeIndex = SO_WeaponInformation.shootingTypeIndex;
        maximumBullets = SO_WeaponInformation.maximumBullets;
        clipSize = SO_WeaponInformation.clipSize;
        bulletsInClip = SO_WeaponInformation.bulletsInClip;
        timeBetweenShots = SO_WeaponInformation.timeBetweenShots;
        timeReload = SO_WeaponInformation.timeReload;

        // Get's the camera
        cam = Camera.main;
    }


    public void WeaponUpdate()
    {
        if (isWeaponActive && !isReloading)
        {

            if (Input.GetMouseButton(0))
            {
                TypeOfShooting();
            }
            if (Input.GetMouseButtonUp(0))
            {
                reShot = true;
                armsAnimator.SetBool("isShooting", false);
                isSpraying = false;
                bulletsFired = 0;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();

            }
            FireRate();
        }
    }

    /// <summary>
    /// Function that calculates the weapon spray.
    /// If Set to false the first bullet will be accurate after that the spray will begin.
    /// </summary>
    /// <param name="isToSpray">If is to reset the spray</param>
    private void CalculateSpray(bool isToSpray)
    {
        if (isToSpray)
        {
            spray = new Vector3(Camera.main.transform.forward.x + Random.Range(-xSpray, xSpray),
                            Camera.main.transform.forward.y + Random.Range(-ySpray, ySpray),
                            Camera.main.transform.forward.z + Random.Range(-zSpray, zSpray)
                            );
        }
        else
        {
            spray = Camera.main.transform.forward;
        }


    }

    private void Shoot()
    {

        bulletParticleSystem.SetActive(true); // Enable Particle System
        audioShoot.Play(); // shoot sound by tiago <----------------------------------------------------------

        armsAnimator.SetTrigger("shoot");
        armsAnimator.SetBool("isShooting", true);

        bulletsFired++;

        if (bulletsFired >= 4)
        {
            isSpraying = true;
        }

        CalculateSpray(isSpraying);

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, spray, out hit))
        {

            // On Hit instantiate Particle Effects 'On Hit' and Destroys after 1 second or so...
            GameObject tempHit = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(tempHit, 1f);


            // Calculate in which direction the bullet has hit and then recalculate a fix value so the decal won't appear "flickering" with other textures
            // Probably use something around 0.001f to fix the flickering
            Vector3 decalNewPosition = new Vector3((hit.point.x + hit.normal.x / 1000), (hit.point.y + hit.normal.y / 1000), (hit.point.z + hit.normal.z / 1000));

            // Debug.Log(hit.transform.name);

            // Now checks if the thing that we hit is a destructable or not (doesn't make sense creating a decal on a object that will be changed...)
            if (hit.transform.GetComponent<IDamage>() != null)
            {
                hit.transform.GetComponent<IDamage>().TakeDamage();
                bulletsInClip--;
                timeElapsedSinceShot = timeBetweenShots;
                canShoot = false;
                return;
            }

            GameObject randomDecal;

            // Ok Seems I will hit something "harder" so let's see what am I hitting...
            switch (hit.transform.tag)
            {
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




    #region Weapon Methods

    /// <summary>
    /// Method that defines what type of shooting will be done.
    /// Controls whether is a Automatic, Brust or Single Shot
    /// </summary>
    private void TypeOfShooting()
    {

        if (!HasBulletsInClip()) return;

        switch (shootingType[shootingTypeIndex])
        {
            case ShootingType.Single:
                if (canShoot && reShot)
                {
                    Shoot();
                    reShot = false;
                }
                break;
            case ShootingType.Burst:
                if (canShoot && !isBursting && reShot)
                {
                    StartCoroutine(BurstShooting());
                    isBursting = true;
                    reShot = false;
                }
                break;
            case ShootingType.Automatic:
                if (canShoot)
                {
                    Shoot();
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
        /*
        //If Player didn't Press the Reload Key, return.
        if (!Input.GetKeyDown(KeyCode.R)) {
            return;
        }
        */
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

        audioReload.Play(); // reload sound by tiago <-------------------------------------------------------------------------

        isReloading = true;

        StartCoroutine(ReloadWeapon(timeReload));

    }

    private IEnumerator ReloadWeapon(float time)
    {
        bulletParticleSystem.SetActive(false); // Enable Particle System
        armsAnimator.SetTrigger("Reload");
        yield return new WaitForSeconds(time);
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
        isReloading = false;
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
        bulletParticleSystem.SetActive(false);
        //weaponAnimator.SetBool("isShooting", false);

    }

    private bool HasBulletsInClip()
    {
        if (bulletsInClip <= 0)
        {
            armsAnimator.SetBool("isShooting", false);
            return false;
        }
        return true;
    }

    #endregion

    #region PUBLIC METHODS

    /// <summary>
    /// Method that activate the Weapon;
    /// </summary>
    /// <param name="isChangingWeapon"></param>
    public void SetActiveWeapon(bool isChangingWeapon)
    {
        isWeaponActive = isChangingWeapon;
        isReloading = false;
    }

    /// <summary>
    /// Method that returns the Weapon Name
    /// </summary>
    /// <returns>Weapon Name</returns>
    public string GetWeaponName()
    {
        return SO_WeaponInformation.name;
    }


    #endregion

    #region COROUTINES

    /// <summary>
    /// Coroutine to shoot in brust
    /// </summary>
    /// <returns></returns>
    private IEnumerator BurstShooting()
    {
        for (int i = 0; i < 3; i++)
        {
            Shoot();
            yield return new WaitForSeconds(timeBetweenShots);
        }
        isBursting = false;
    }

    #endregion
}
