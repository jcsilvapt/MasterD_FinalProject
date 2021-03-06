using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class charController : MonoBehaviour, IDamage
{

    [Header("Player Settgins")]
    [SerializeField] float health = 100;
    [SerializeField] float maxHealth = 100;
    [SerializeField] float moveSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float crouchSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] cameraRotation cam;
    [SerializeField] bool canMove;
    private bool isAlive;

    [Header("Player Health Effect")]
    [SerializeField] Material healthEmission;
    [SerializeField] GameObject armsMesh;
    [SerializeField] GameObject bodyMesh;
    private float healthC;
    private Color healthColor;

    [Header("Player Settings: Weapon Controller")]
    [SerializeField] WeaponController weaponController;
    /*[SerializeField] */
    public bool hasWeapon;

    [Header("Player Settings: Steps Climbing")]
    [SerializeField] bool enableStairsWalk = false;
    [SerializeField] GameObject stepHigh;
    [SerializeField] GameObject stepLow;
    [SerializeField] float stepSmooth = 0.1f;


    [Header("Player Settings: Crouch")]
    [SerializeField] Transform character;
    [SerializeField] float minDistanceToStandUp = 0.5f;
    [SerializeField] float characterCrouchHeight;
    [SerializeField] float cameraCrouchHeight;
    [SerializeField] float cameraCrouchLerpSpeed = 0.5f;
    private float cameraDefaultHeight;
    private float characterDefaultHeight;

    [Header("Player Settings: Drone")]
    /*[SerializeField]*/
    public bool canUseDrone = false;
    [SerializeField] private Transform droneSpawn;
    [SerializeField] private Transform drone;
    [SerializeField] GameObject playerDroneBody;


    [Header("Player Settings: Animators and GameObjects")]
    [SerializeField] GameObject arms;
    [SerializeField] GameObject body;
    [SerializeField] SkinnedMeshRenderer physicalBodyMesh1;
    [SerializeField] SkinnedMeshRenderer physicalBodyMesh2;
    [SerializeField] Animator bodyAnim;


    [Header("Player Settings: Step Sounds")]
    [SerializeField] Terrain_Behaviour terrainBehaviour;

    [Header("Player Settings: Player UI")]
    [SerializeField] GameObject uiCrosshair;
    [SerializeField] GameObject uiInteraction;


    [Header("DEVELOPER SETTINGS")]
    [SerializeField] bool isRunning;
    [SerializeField] bool isCrouched;
    [SerializeField] bool isGrounded;
    [SerializeField] bool isDroneActive;
    public Checkpoint lastCheckpoint;
    [SerializeField] bool lockPause = false;

    [Header("Sound Effects")]
    [SerializeField] AudioSource steps;
    [SerializeField] float crouchPitch;
    [SerializeField] float runPitch;
    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioMixerSnapshot[] snapShot;
    [SerializeField] float[] snapNumber;

    [Header("Extras Stuff")]
    [SerializeField] Rigidbody rb;
    [SerializeField] Camera fpsCam;
    [SerializeField] GameObject armaAtual;
    [SerializeField] float radius;
    [Tooltip("Check if player is still stealth or if enemy saw him")]
    public bool isStealth = true;
    [SerializeField] CanvasGroup screenRed;


    void Start()
    {
        // Get References
        rb = GetComponent<Rigidbody>();
        cameraDefaultHeight = fpsCam.GetComponent<Transform>().localPosition.y;
        characterDefaultHeight = character.localScale.y;

        // When the game begins it needs to hide the Arms
        if (hasWeapon)
        {
            weaponController.EnableWeapon();
        }
        arms.SetActive(hasWeapon);

        // Disables SkinnedMeshRenderer only to cast shadows
        physicalBodyMesh1.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        physicalBodyMesh2.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

        healthEmission = armsMesh.GetComponent<SkinnedMeshRenderer>().sharedMaterial;

        playerDroneBody.SetActive(false);

        canMove = true;
        isAlive = true;
    }

    void Update()
    {
        // TESTING SAVE SYSTEM
        if (!isAlive)
        {
            return;
        }

        if (health <= 0 && isAlive)
        {
            isAlive = false;
            StopMovement();
            GameManager.SetPlayerDeath(true);
            return;
        }

        if (!GameManager.GetPause())
        {

            if (Input.GetKeyDown(KeyMapper.inputKey.Escape) && !lockPause)
            {
                GameManager.SetPause();
            }

            if (canMove)
            {
                healthEmission.SetColor("_EmissionColor", healthColor * 3);
                healthColor = Color.Lerp(Color.green, Color.red * 3, healthC);

                if (isDroneActive)
                {
                    steps.mute = true;
                    screenRed.alpha = 0;
                }
                else
                {
                    steps.mute = false;
                    screenRed.alpha = 1 - (health / 100);
                }

                if (!isDroneActive)
                {
                    Movement();

                    Crouch();

                    Jump();

                    CheckStateForSounds();

                    if (enableStairsWalk)
                    {
                        StepClimb();
                    }
                }
                if (canUseDrone && isGrounded)
                    DroneControl();

                CheckStealthiness();
            }
        }
    }

    #region Movement Functions
    private void Crouch()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(character.transform.position + Vector3.up, Vector3.up, out hitInfo, minDistanceToStandUp))
        {
            if (hitInfo.transform.tag != "Armory")
                isCrouched = true;
        }
        else
        {
            isCrouched = Input.GetKey(KeyMapper.inputKey.Crouch);
        }

        float cameraTempCrouchHeight = isCrouched ? cameraCrouchHeight : cameraDefaultHeight;
        float charTempCrouchHeight = isCrouched ? characterCrouchHeight : characterDefaultHeight;

        float cameraNewY = Mathf.Lerp(fpsCam.transform.localPosition.y, cameraTempCrouchHeight, cameraCrouchLerpSpeed);
        float charNewY = Mathf.Lerp(character.transform.localScale.y, charTempCrouchHeight, cameraCrouchLerpSpeed);

        fpsCam.transform.localPosition = new Vector3(fpsCam.transform.localPosition.x, cameraNewY, fpsCam.transform.localPosition.z);
        character.transform.localScale = new Vector3(character.transform.localScale.x, charNewY, character.transform.localScale.z);
    }
    void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        isRunning = Input.GetKey(KeyMapper.inputKey.Sprint);

        float tempMoveSpeed = isCrouched ? crouchSpeed : isRunning ? runSpeed : moveSpeed;

        Vector3 movePos = transform.right * x + transform.forward * z;

        movePos = movePos.normalized;


        if (movePos != Vector3.zero && isGrounded)
        {
            Vector3 rbVelocity = new Vector3(movePos.x, rb.velocity.y, movePos.z);
            rb.velocity = Vector3.Scale(rbVelocity, new Vector3(tempMoveSpeed, 1, tempMoveSpeed));
            bodyAnim.SetBool("isWalking", true);
        }
        else if (isGrounded)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            bodyAnim.SetBool("isWalking", false);
        }

        if (!isGrounded)
        {
            steps.mute = true;
            if (rb.velocity.x == 0 && rb.velocity.z == 0)
            {
                return;
            }

            if (rb.velocity.z > 0)
            {
                if (movePos.z > 0)
                {
                    movePos.z = 0;
                }
            }
            else if (rb.velocity.z < 0)
            {
                if (movePos.z < 0)
                {
                    movePos.z = 0;
                }
            }

            if (rb.velocity.x > 0)
            {
                if (movePos.x > 0)
                {
                    movePos.x = 0;
                }
            }
            else if (rb.velocity.x < 0)
            {
                if (movePos.x < 0)
                {
                    movePos.x = 0;
                }
            }

            rb.velocity = new Vector3(rb.velocity.x + (movePos.x * 0.1f), rb.velocity.y, rb.velocity.z + (movePos.z * 0.1f));
        }
    }
    void CheckStateForSounds() //uses audioMixer SnapShots to switch between all three states of movement, walking,
                               //running and crouching by switching their weight from 0 to 1 for most value  
    {
        if (isCrouched)
        {
            snapNumber[0] = 1f;
            snapNumber[1] = 0f;
            snapNumber[2] = 0f;
            mixer.TransitionToSnapshots(snapShot, snapNumber, 0f);
        }
        else if (isRunning)
        {
            snapNumber[0] = 0f;
            snapNumber[1] = 1f;
            snapNumber[2] = 0f;
            mixer.TransitionToSnapshots(snapShot, snapNumber, 0f);
        }
        else
        {
            snapNumber[0] = 0f;
            snapNumber[1] = 0f;
            snapNumber[2] = 1f;
            mixer.TransitionToSnapshots(snapShot, snapNumber, 0f);
        }
    }
    void Jump()
    {
        //salta
        if (isCrouched == false && isGrounded == true) //pode-se alterar, criado por questoes de testes(saltar enquanto crouch)
        {
            if (Input.GetKeyDown(KeyMapper.inputKey.Jump))
            {
                rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            }
        }
    }
    void StepClimb() //not in use but working
    {        
        RaycastHit hitLower;
        RaycastHit hitLower45;
        RaycastHit hitLower90;

        if (Physics.Raycast(stepLow.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.1f))
        {
            RaycastHit hitHigher;
            if (!Physics.Raycast(stepHigh.transform.position, transform.TransformDirection(Vector3.forward), out hitHigher, 0.2f)) 
            {
                rb.position -= new Vector3(0f, -stepSmooth, 0f); //the bigger the stepsmooth value the bigger the jump
            }
        }
        else if (Physics.Raycast(stepLow.transform.position, transform.TransformDirection(2f, 0, 1), out hitLower45, 0.1f))
        {
            RaycastHit hitHigher45;
            if (!Physics.Raycast(stepHigh.transform.position, transform.TransformDirection(2f, 0, 1), out hitHigher45, 0.2f))
            {
                rb.position -= new Vector3(0f, -stepSmooth, 0f);
            }
        }
        else if (Physics.Raycast(stepLow.transform.position, transform.TransformDirection(-2f, 0, 1), out hitLower90, 0.1f))
        {
            RaycastHit hitHigher90;
            if (!Physics.Raycast(stepHigh.transform.position, transform.TransformDirection(-2f, 0, 1), out hitHigher90, 0.2f))
            {
                rb.position -= new Vector3(0f, -stepSmooth, 0f);
            }
        }

    }

    #region isGrounded
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "cenario" || collision.gameObject.tag == "Wood" || collision.gameObject.tag == "Metal" || collision.gameObject.tag == "NotHideable")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
    #endregion

    public void StartMovement()
    {
        canMove = true;
        weaponController.ResumeWeapons();
    }

    public void StopMovement()
    {
        canMove = false;
        rb.velocity = Vector3.zero;
        bodyAnim.SetBool("isWalking", false);
        steps.mute = true;
        weaponController.StopWeapons();
    }

    #endregion

    #region Drone
    private void DroneControl()
    {
        if (Input.GetKeyDown(KeyMapper.inputKey.DroneActivation))
        {
            if (!isDroneActive)
            {
                drone.transform.SetParent(null);

                drone.gameObject.SetActive(true);

                drone.GetComponent<DroneController>().SetTransform(droneSpawn.position, transform.eulerAngles);

                fpsCam.gameObject.SetActive(false);

                isDroneActive = true;

                // Checks if has a weapon, and if it has needs to disable otherwise will try to shoot with the weapon aswell
                if (weaponController.HasWeapon())
                {
                    weaponController.DisableCurrentWeapon();
                }

                playerDroneBody.SetActive(true);

            }
            else
            {
                fpsCam.gameObject.SetActive(true);

                drone.transform.SetParent(droneSpawn);

                drone.GetComponent<DroneController>().ResetTransform();

                drone.gameObject.SetActive(false);

                isDroneActive = false;
                if (weaponController.HasWeapon())
                {
                    weaponController.EnableWeapon();
                }

                playerDroneBody.SetActive(false);
            }
        }
    }
    /// <summary>
    /// Public function that can enable or disable when the player has acess to the drone.
    /// </summary>
    /// <param name="value">True enables drone control | False disables drone control</param>
    public void SetDroneControl(bool value)
    {
        canUseDrone = value;
        Debug.LogWarning("You can now use the drone by pressing the key: '" + KeyMapper.inputKey.DroneActivation.ToString() + "'");
    }
    #endregion

    #region PUBLIC ACESS

    public void EnableWeapon()
    {
        hasWeapon = true;
        weaponController.EnableWeapon();
        arms.SetActive(true);
    }

    public void TakeDamage()
    {
        health -= 5;
        if (health > 0)
        {
            ScreenDamageColor();
            healthC += 0.05f;
        }
    }

    public void CheckStealthiness()
    {
        if (health < 100)
        {
            isStealth = false;
        }
    }
    private void CheckHealth()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public float GetHealth()
    {
        return health;
    }

    public Checkpoint GetLastCheckpoint()
    {
        return lastCheckpoint;
    }

    public void EnableInteractionUI(bool enableUI)
    {
        uiCrosshair.SetActive(!enableUI);
        uiInteraction.GetComponentInChildren<Text>().text = "PRESS '" + KeyMapper.inputKey.Interaction + "' TO INTERACT";
        uiInteraction.SetActive(enableUI);
    }

    /// <summary>
    /// X: Maximum Bullets | Y: Current Bullets
    /// </summary>
    /// <returns></returns>
    public Vector2 GetCurrentWeaponBullets()
    {
        return weaponController.GetCurrentWeaponAmmo();
    }

    public void LockPause()
    {
        lockPause = true;
    }

    public void UnlockPause()
    {
        lockPause = false;
    }

    #region

    public void ReplenishHealth()
    {
        health = maxHealth;
        healthC = 0;

        ScreenDamageColor();
    }

    public void ReplenishBullets()
    {
        foreach (Weapon weapon in weaponController.GetPlayerWeapons())
        {
            weapon.ReplenishBullets();
        }
    }

    #endregion

    #endregion

    #region Extras

    // Used when player touches health pack or ammo pack, get some ammo or some health
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Health")
        {
            health += 50;
            CheckHealth();
            ScreenDamageColor();
            healthC -= .5f;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Ammo")
        {
            foreach (Weapon weapon in weaponController.GetPlayerWeapons())
            {
                weapon.AddBullets(20);
            }

            Destroy(other.gameObject);
        }
    }

    private void ScreenDamageColor()
    {
        /* Color c = screenDamage.color;
         c.a = 1 - (health / 100);
         screenDamage.color = c;*/
        screenRed.alpha = 1 - (health / 100);
    }
    #endregion

    #region Fabio Edit

    public int[] GetWeaponsAmmunition()
    {
        return new int[] { weaponController.GetTotalCurrentAmmunition(), weaponController.GetTotalMaximumAmmunition() };
    }

    #endregion
}
