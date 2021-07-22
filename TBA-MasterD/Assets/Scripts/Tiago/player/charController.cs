using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charController : MonoBehaviour {
    [Header("Stairs Walk")]
    public bool enableStairsWalk = false;
    public GameObject stepHigh;
    public GameObject stepLow;
    public float stepHeight;
    public float stepSmooth;


    [Header("Extras")]
    Rigidbody rb;
    public Camera fpsCam;
    public GameObject armaAtual;
    public float radius;


    [Header("Movimento")]
    public float moveSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public float jumpHeight;


    [Header("Crouch Settings")]
    public Transform character;
    public float minDistanceToStandUp = 0.5f;
    public float characterCrouchHeight;
    public float cameraCrouchHeight;
    public float cameraCrouchLerpSpeed = 0.5f;
    private float cameraDefaultHeight;
    private float characterDefaultHeight;

    [Header("Estados")]
    public bool isRunning;
    public bool isCrouched;
    public bool isGrounded;

    [Range(0.0f, 5.0f)]
    public float distance;

    [Header("Vida e Extras")]
    public int Health = 100;

    //MEGA TESTES
    private bool isActive;
    [SerializeField] private bool canUseDrone = false;
    [SerializeField] private Transform droneSpawn;
    [SerializeField] private Transform drone;

    void Start() {
        rb = GetComponent<Rigidbody>();
        cameraDefaultHeight = fpsCam.GetComponent<Transform>().localPosition.y;
        characterDefaultHeight = character.localScale.y;

        //MEGA TESTES
        isActive = true;
    }


    void Update() {

        if (isActive) {
            Movement();

            Chrouch();

            Jump();

            if (enableStairsWalk) {
                StepClimb();
            }
        }
        if(canUseDrone)
            DroneControl();
    }

    private void Chrouch() {

        RaycastHit hitInfo;
        //character.transform.position + Vector3.up, Vector3.up, hitInfo, minDistanceToStandUp, 0)
        if (Physics.Raycast(character.transform.position + Vector3.up, Vector3.up, out hitInfo, minDistanceToStandUp)) {
            if(hitInfo.transform.tag != "Armory")
                isCrouched = true;
        } else {
            isCrouched = Input.GetKey(KeyCode.LeftControl);
        }

        float cameraTempCrouchHeight = isCrouched ? cameraCrouchHeight : cameraDefaultHeight;
        float charTempCrouchHeight = isCrouched ? characterCrouchHeight : characterDefaultHeight;

        float cameraNewY = Mathf.Lerp(fpsCam.transform.localPosition.y, cameraTempCrouchHeight, cameraCrouchLerpSpeed);
        float charNewY = Mathf.Lerp(character.transform.localScale.y, charTempCrouchHeight, cameraCrouchLerpSpeed);

        fpsCam.transform.localPosition = new Vector3(fpsCam.transform.localPosition.x, cameraNewY, fpsCam.transform.localPosition.z);
        character.transform.localScale = new Vector3(character.transform.localScale.x, charNewY, character.transform.localScale.z);




    }



    void Movement() {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        isRunning = Input.GetKey(KeyCode.LeftShift);

        float tempMoveSpeed = isCrouched ? crouchSpeed : isRunning ? runSpeed : moveSpeed;

        Vector3 movePos = transform.right * x + transform.forward * z;

        movePos = movePos.normalized;


        if (movePos != Vector3.zero && isGrounded) {
            Vector3 rbVelocity = new Vector3(movePos.x, rb.velocity.y, movePos.z);
            rb.velocity = Vector3.Scale(rbVelocity, new Vector3(tempMoveSpeed, 1, tempMoveSpeed));
        } else if (isGrounded) {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        if (!isGrounded) {

            if (rb.velocity.x == 0 && rb.velocity.z == 0) {
                return;
            }

            if (rb.velocity.z > 0) {
                if (movePos.z > 0) {
                    movePos.z = 0;
                }
            } else if (rb.velocity.z < 0) {
                if (movePos.z < 0) {
                    movePos.z = 0;
                }
            }

            if (rb.velocity.x > 0) {
                if (movePos.x > 0) {
                    movePos.x = 0;
                }
            } else if (rb.velocity.x < 0) {
                if (movePos.x < 0) {
                    movePos.x = 0;
                }
            }

            rb.velocity = new Vector3(rb.velocity.x + (movePos.x * 0.1f), rb.velocity.y, rb.velocity.z + (movePos.z * 0.1f));
        }

    } // andar correr e crouch



    void Jump() {
        //salta
        if (isCrouched == false && isGrounded == true) //pode-se alterar, criado por questoes de testes(saltar enquanto crouch)
        {
            if (Input.GetKeyDown(KeyCode.Space)) {
                rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            }
        }
    }

    void StepClimb() // 
    {

        RaycastHit hitLower;
        if (Physics.Raycast(stepLow.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.1f)) {
            RaycastHit hitHigher;
            if (!Physics.Raycast(stepHigh.transform.position, transform.TransformDirection(Vector3.forward), out hitHigher, 0.2f)) // so é chamado caso o primeiro atinga algo
            {
                rb.position -= new Vector3(0f, -stepSmooth, 0f); //quanto maior os stepsmooth, mais a personagem salta
            }
        }

        RaycastHit hitLower45;
        if (Physics.Raycast(stepLow.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, 0.1f)) {
            RaycastHit hitHigher45;
            if (!Physics.Raycast(stepHigh.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitHigher45, 0.2f)) {
                rb.position -= new Vector3(0f, -stepSmooth, 0f);
            }
        }

        RaycastHit hitLower90;
        if (Physics.Raycast(stepLow.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLower90, 0.1f)) {
            RaycastHit hitHigher90;
            if (!Physics.Raycast(stepHigh.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitHigher90, 0.2f)) {
                rb.position -= new Vector3(0f, -stepSmooth, 0f);
            }
        }


    } // subir degraus

    #region isGrounded
    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.tag == "cenario") {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision) {
        isGrounded = false;
    }
    #endregion

    //MEGA TESTES
    private void DroneControl() {
        if (Input.GetKeyDown(KeyCode.F)) {
            if (isActive) {
                fpsCam.gameObject.SetActive(false);

                drone.gameObject.SetActive(true);

                isActive = false;
            } else {
                fpsCam.gameObject.SetActive(true);


                drone.GetComponent<DroneController>().ResetTransform();

                drone.gameObject.SetActive(false);

                isActive = true;
            }
        }
    }

    #region PUBLIC ACESS
    /// <summary>
    /// Public function that can enable or disable when the player has acess to the drone.
    /// </summary>
    /// <param name="value">True enables drone control | False disables drone control</param>
    public void SetDroneControl(bool value) {
        canUseDrone = value;
    }

    #endregion
}
