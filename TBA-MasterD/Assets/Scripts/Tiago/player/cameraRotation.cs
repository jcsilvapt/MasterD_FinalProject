using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotation : MonoBehaviour {
    //movimentos do Rato

    public float mouseX;
    public float mouseY;
    [Tooltip("This setting is now controlled by the Keymapper controller")]
    [SerializeField] bool invertMouse;
    private float mouseMultiplier = 100f; // This value is used to standard the mouseSensitivity values

    //corpo do jogador
    public Transform playerBody;
    private float xRotation = 0f;

    [Header("IK Settings")]
    [SerializeField] private Vector3 headOffset;
    [SerializeField] private Vector3 chestOffset;
    [Header("Right Arm")]
    [SerializeField] private Vector3 rightUpperArmOffset;
    [SerializeField] private Vector3 rightLowerArmOffset;
    [SerializeField] private Vector3 rightHandOffset;
    [Header("Left Arm")]
    [SerializeField] private Vector3 leftUpperArmOffset;
    [SerializeField] private Vector3 leftLowerArmOffset;
    [SerializeField] private Vector3 leftHandOffset;

    [SerializeField] Transform ikRightHand, ikLeftHand, target;
    [SerializeField] Animator animator;
    private Transform chest, head;
    private Transform rightUpperArm, rightLowerArm, rightHand;
    private Transform leftUpperArm, leftLowerArm, leftHand;
    private Transform leftArm;

    void Start() {
        //mouseX = playerBody.eulerAngles.y;
    }
   

    private void FixedUpdate() {
       if (!GameManager.GetCursorVisibility()) {
            GetMouseMovement();
            RotateCamera();
        }
    }

    private void RotateCamera() {
        playerBody.Rotate(Vector3.up * mouseX);
        transform.localRotation = Quaternion.Euler(mouseY, 0, 0);

    }

    private void GetMouseMovement() {

        float sensitivity = KeyMapper.inputKey.MouseSensitivity;

        mouseX = Input.GetAxis("Mouse X") * sensitivity;
        if (KeyMapper.inputKey.InvertMouse) {
            mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        } else {
            mouseY -= Input.GetAxis("Mouse Y") * sensitivity;
        }
        mouseY = Mathf.Clamp(mouseY, -90, 90);

    }
    
}
