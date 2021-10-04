using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour {
    #region References

    //Rigidbody Reference
    private Rigidbody rb;

    //Dart Reference
    [SerializeField] private GameObject dart;

    //Shooting Point
    [SerializeField] private Transform shootingPoint;

    //DroneCamera
    [SerializeField] private Camera droneCamera;

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


    //dart shoot sound
    public AudioClip dartShootSound;
    public AudioSource dartSound;
    #endregion

    private void Start() {
        //Get References
        rb = GetComponent<Rigidbody>();

        //Set Shoot Timer
        shootTimer = 0;
    }

    private void Update() {

        if (!GameManager.GetPause()) {

            Movement();

            Look();

            Shoot();

        }
    }

    private void FixedUpdate() {
        if (movementInput.magnitude != 0 || heightController.magnitude != 0) {
            rb.velocity = (transform.TransformDirection(movementInput) + heightController).normalized * movementSpeed;
        } else {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void Movement() {
        //Movement Input by Player
        movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        //Height Controller Changes
        if (Input.GetKey(KeyMapper.inputKey.DroneMoveUp)) {
            heightController.y = 0.5f;
        } else if (Input.GetKey(KeyMapper.inputKey.DroneMoveDown)) {
            heightController.y = -0.5f;
        } else {
            heightController.y = 0;
        }

        //Normalize Movement
        movementInput = movementInput.normalized;
    }

    private void Look() {
        xRotation += Input.GetAxis("Mouse X") * droneSensitivity;
        yRotation -= Input.GetAxis("Mouse Y") * droneSensitivity;

        transform.localEulerAngles = new Vector3((yRotation = Mathf.Clamp(yRotation, rotationLimitMinY, rotationLimitMaxY)), xRotation, 0);
    }

    private void Shoot() {
        if (shootTimer > 0) {
            shootTimer -= Time.deltaTime;
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            dartSound.PlayOneShot(dartShootSound);
            RaycastHit hit;
            if (Physics.Raycast(droneCamera.transform.position, droneCamera.transform.forward, out hit)) {
                Debug.Log("DRONE::: " + hit.transform.GetComponent<IDamage>());
                if (hit.transform.GetComponent<IDamage>() != null) {
                    if (!hit.transform.CompareTag("Enemy"))
                    {
                        hit.transform.GetComponent<IDamage>().TakeDamage();
                    }
                }
            }

            //GameObject dartShot = Instantiate(dart, shootingPoint);
            //dartShot.transform.parent = null;

            shootTimer = timeBetweenShots;
        }
    }

    public void ResetTransform() {
        //Reset Rotation
        xRotation = 0;
        yRotation = 0;

        //Reset Position
        transform.localPosition = Vector3.zero;
    }

    public void SetTransform(Vector3 position, Vector3 rotation) {
        transform.position = position;
        transform.eulerAngles = Vector3.Scale(Vector3.up, rotation);

        xRotation = rotation.y;
        yRotation = rotation.x;
    }

    private void OnEnable() {
        Debug.LogWarning("Keys to interact: " + KeyMapper.inputKey.DroneMoveUp.ToString() + ", to go UP and, " + KeyMapper.inputKey.DroneMoveDown.ToString() + ", to go down.");
    }
}
