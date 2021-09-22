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
        /*head = animator.GetBoneTransform(HumanBodyBones.Head);
        chest = animator.GetBoneTransform(HumanBodyBones.Chest);
        rightUpperArm = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
        rightLowerArm = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
        rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);

        leftUpperArm = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        leftLowerArm = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);*/
        mouseX = playerBody.eulerAngles.y;
    }


    void Update() {

        if (Input.GetKeyDown(KeyCode.K)) {
            GameManager.SetCursorVisibility();
        }
        /*
        if (!GameManager.GetCursorVisibility()) {
            mouseX += Input.GetAxis("Mouse X") * (KeyMapper.inputKey.MouseSensitivity * mouseMultiplier) * Time.deltaTime;
            Debug.Log("Mouse X: " + mouseX);
            mouseY += Input.GetAxis("Mouse Y") * (KeyMapper.inputKey.MouseSensitivity * mouseMultiplier) * Time.deltaTime;
            Debug.Log("Sense: " + KeyMapper.inputKey.MouseSensitivity * mouseMultiplier);
            mouseY = 0;
            if(KeyMapper.inputKey.InvertMouse) {
                xRotation += mouseY;
            } else {
                xRotation -= mouseY;
            }
            
            xRotation = Mathf.Clamp(xRotation, -90, 90);

            // Roda Câmera no Y
            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            // Roda Câmera no X
            playerBody.Rotate(Vector3.up * mouseX);
        }
        */
    }

    private void FixedUpdate() {
        if (!GameManager.GetCursorVisibility()) {
            GetMouseMovement();
            RotateCamera();
        }
    }

    private void RotateCamera() {
        //Quaternion playerBodyRotate = Quaternion.Euler(0, mouseX, 0);
        Vector3 bodyRotation = new Vector3(0, mouseX, 0);

        playerBody.localRotation = Quaternion.AngleAxis(mouseX, playerBody.transform.up);
        transform.localRotation = Quaternion.AngleAxis(mouseY, Vector3.right);

    }

    private void GetMouseMovement() {

        float sensitivity = KeyMapper.inputKey.MouseSensitivity;
        Debug.Log("Camera Sensitivity: " + sensitivity);

        mouseX += Input.GetAxis("Mouse X") * sensitivity;

        if (KeyMapper.inputKey.InvertMouse) {
            mouseY += Input.GetAxis("Mouse Y") * sensitivity;
        } else {
            mouseY -= Input.GetAxis("Mouse Y") * sensitivity;
        }
        mouseY = Mathf.Clamp(mouseY, -90, 90);

    }

    private void LateUpdate() {
        /*
        chest.LookAt(target.position);
        chest.rotation = chest.rotation * Quaternion.Euler(chestOffset);

        head.LookAt(target.position);
        head.rotation = head.rotation * Quaternion.Euler(headOffset);
        */
        /*
        // Right Arm
        rightUpperArm.LookAt(target.position);
        rightUpperArm.rotation = rightUpperArm.rotation * Quaternion.Euler(rightUpperArmOffset);

        rightLowerArm.LookAt(target.position);
        rightLowerArm.rotation = rightLowerArm.rotation * Quaternion.Euler(rightLowerArmOffset);

        rightHand.LookAt(target.position);
        rightHand.rotation = rightHand.rotation * Quaternion.Euler(rightHandOffset);


        // Left Arm
        leftUpperArm.LookAt(target.position);
        leftUpperArm.rotation = leftUpperArm.rotation * Quaternion.Euler(leftUpperArmOffset);

        leftLowerArm.LookAt(target.position);
        leftLowerArm.rotation = leftLowerArm.rotation * Quaternion.Euler(leftLowerArmOffset);

        leftHand.LookAt(target.position);
        leftHand.rotation = leftHand.rotation * Quaternion.Euler(leftHandOffset);
        */
    }
}
