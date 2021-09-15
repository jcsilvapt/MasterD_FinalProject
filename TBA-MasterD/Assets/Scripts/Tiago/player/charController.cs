using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class charController : MonoBehaviour
{

    [Header("Player Settgins")]
    [SerializeField] int Health = 100;
    [SerializeField] float moveSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float crouchSpeed;
    [SerializeField] float jumpHeight;

    [Header("Player Settings: Weapon Controller")]
    [SerializeField] WeaponController weaponController;
    [SerializeField] bool hasWeapon;

    [Header("Player Settings: Steps Climbing")]
    [SerializeField] bool enableStairsWalk = false;
    [SerializeField] GameObject stepHigh;
    [SerializeField] GameObject stepLow;
    [SerializeField] float stepHeight;
    [SerializeField] float stepSmooth;

    [Header("Player Settings: Crouch")]
    [SerializeField] Transform character;
    [SerializeField] float minDistanceToStandUp = 0.5f;
    [SerializeField] float characterCrouchHeight;
    [SerializeField] float cameraCrouchHeight;
    [SerializeField] float cameraCrouchLerpSpeed = 0.5f;
    private float cameraDefaultHeight;
    private float characterDefaultHeight;

    [Header("Player Settings: Drone")]
    [SerializeField] private bool canUseDrone = false;
    [SerializeField] private Transform droneSpawn;
    [SerializeField] private Transform drone;


    [Header("Player Settings: Animators and GameObjects")]
    [SerializeField] GameObject arms;
    [SerializeField] GameObject body;
    [SerializeField] SkinnedMeshRenderer physicalBodyMesh1;
    [SerializeField] SkinnedMeshRenderer physicalBodyMesh2;
    [SerializeField] Animator bodyAnim;


    [Header("Player Settings: Step Sounds")]
    [SerializeField] Terrain_Behaviour terrainBehaviour;


    [Header("DEVELOPER SETTINGS")]
    [SerializeField] bool isRunning;
    [SerializeField] bool isCrouched;
    [SerializeField] bool isGrounded;
    [SerializeField] bool isDroneActive;

    [Header("Sound Effects")]
    public AudioSource steps;
    [SerializeField] float crouchPitch;
    [SerializeField] float runPitch;
    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioMixerSnapshot[] snapShot;
    [SerializeField] float[] snapNumber;

    [Header("Extras Stuff")]
    [SerializeField] Rigidbody rb;
    public Camera fpsCam;
    public GameObject armaAtual;
    public float radius;



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

    }

    void Update()
    {
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
        if (canUseDrone)
            DroneControl();
    }

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
    void CheckStateForSounds() //uses audioMixer SnapShots to switch between all three states of movemente, walking, running and crouching by switching their weight from 0 to 1 for most value  
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            }
        }
    }
    void StepClimb()
    {// uses two raycasts to measure height of objects in front and determine is the character can climb stairs up with a tiny jump or not

        RaycastHit hitLower;
        if (Physics.Raycast(stepLow.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.1f))
        {
            RaycastHit hitHigher;
            if (!Physics.Raycast(stepHigh.transform.position, transform.TransformDirection(Vector3.forward), out hitHigher, 0.2f)) // so é chamado caso o primeiro atinga algo
            {
                rb.position -= new Vector3(0f, -stepSmooth, 0f); //quanto maior os stepsmooth, mais a personagem salta
            }
        }

        RaycastHit hitLower45;
        if (Physics.Raycast(stepLow.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, 0.1f))
        {
            RaycastHit hitHigher45;
            if (!Physics.Raycast(stepHigh.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitHigher45, 0.2f))
            {
                rb.position -= new Vector3(0f, -stepSmooth, 0f);
            }
        }

        RaycastHit hitLower90;
        if (Physics.Raycast(stepLow.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLower90, 0.1f))
        {
            RaycastHit hitHigher90;
            if (!Physics.Raycast(stepHigh.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitHigher90, 0.2f))
            {
                rb.position -= new Vector3(0f, -stepSmooth, 0f);
            }
        }


    } // subir degraus, not in use but working 

    #region isGrounded
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "cenario" || collision.gameObject.tag == "Wood" || collision.gameObject.tag == "Metal")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
    #endregion

    //MEGA TESTES
    private void DroneControl()
    {
        if (Input.GetKeyDown(KeyMapper.inputKey.DroneActivation))
        {
            if (!isDroneActive)
            {
                fpsCam.gameObject.SetActive(false);

                drone.gameObject.SetActive(true);

                isDroneActive = true;

                // Checks if has a weapon, and if it has needs to disable otherwise will try to shoot with the weapon aswell
                if (weaponController.HasWeapon())
                {
                    weaponController.DisableCurrentWeapon();
                }

            }
            else
            {
                fpsCam.gameObject.SetActive(true);


                drone.GetComponent<DroneController>().ResetTransform();

                drone.gameObject.SetActive(false);

                isDroneActive = false;
                if (weaponController.HasWeapon())
                {
                    weaponController.EnableWeapon();
                }
            }
        }
    }

    #region PUBLIC ACESS
    /// <summary>
    /// Public function that can enable or disable when the player has acess to the drone.
    /// </summary>
    /// <param name="value">True enables drone control | False disables drone control</param>
    public void SetDroneControl(bool value)
    {
        canUseDrone = value;
        Debug.LogWarning("You can now use the drone by pressing the key: '" + KeyMapper.inputKey.DroneActivation.ToString() + "'");
    }

    public void EnableWeapon()
    {
        hasWeapon = true;
        weaponController.EnableWeapon();
        arms.SetActive(true);
    }

    #endregion

}
