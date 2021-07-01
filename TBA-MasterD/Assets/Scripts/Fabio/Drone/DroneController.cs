using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    #region References

    //Rigidbody Reference
    private Rigidbody rb;

    //Dart Reference
    [SerializeField] private GameObject dart;

    //Shooting Point
    [SerializeField] private Transform shootingPoint;

    #endregion

    #region Movement Variables

    //Drone Movement Vector
    private Vector3 movementInput;

    //Drone Height Controller
    private Vector3 heightController;

    //Drone Speed
    [SerializeField] private float movementSpeed;

    #endregion

    #region Look Variables

    //How fast the camera rotates
    [SerializeField] private float droneSensitivity;

    //Stacking Rotation
    private float xRotation;
    private float yRotation;

    //Camera Rotation Limits
    [SerializeField] private float rotationLimitMinY;
    [SerializeField] private float rotationLimitMaxY;

    #endregion

    #region Shooting Variables

    //Time it Takes between Shots
    [SerializeField] private float timeBetweenShots;

    //Timer keeping track of the times
    private float shootTimer;

    #endregion

    private void Start()
    {
        //Get References
        rb = GetComponent<Rigidbody>();

        //Set Shoot Timer
        shootTimer = 0;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Movement();

        Look();

        Shoot();
    }

    private void FixedUpdate()
    {
        rb.velocity = (transform.TransformDirection(movementInput) + heightController).normalized * movementSpeed;
        //rb.velocity += heightController * movementSpeed;
    }

    private void Movement()
    {
        //Movement Input by Player
        movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        //Height Controller Changes
        if (Input.GetKey(KeyCode.E))
        {
            heightController.y = 0.5f;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            heightController.y = -0.5f;
        }
        else
        {
            heightController.y = 0;
        }

        //Normalize Movement
        movementInput = movementInput.normalized;
    }

    private void Look()
    {
        xRotation += Input.GetAxis("Mouse X") * droneSensitivity;
        yRotation -= Input.GetAxis("Mouse Y") * droneSensitivity;

        transform.localEulerAngles = new Vector3((yRotation = Mathf.Clamp(yRotation, rotationLimitMinY, rotationLimitMaxY)), xRotation, 0);
    }

    private void Shoot()
    {
        if(shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            GameObject dartShot = Instantiate(dart, shootingPoint);
            dartShot.transform.parent = null;

            shootTimer = timeBetweenShots;
        }
    }
}
