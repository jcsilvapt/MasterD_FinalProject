using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotation : MonoBehaviour {
    //movimentos do Rato
    private float mouseX;
    private float mouseY;
    public float mouseSensitivy = 100f;

    public bool showCursor = true;

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
        Debug.LogWarning("To Hide the mouse cursor just press 'K'");
        /*head = animator.GetBoneTransform(HumanBodyBones.Head);
        chest = animator.GetBoneTransform(HumanBodyBones.Chest);
        rightUpperArm = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
        rightLowerArm = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
        rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);

        leftUpperArm = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        leftLowerArm = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);*/
    }


    void Update() {

        if (Input.GetKeyDown(KeyCode.K)) {
            ToggleCursorVisibility();
        }

        if (!showCursor) {
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivy * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivy * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90, 90);

            // Roda Câmera no Y
            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            // Roda Câmera no X
            playerBody.Rotate(Vector3.up * mouseX);
        }

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

    private void ToggleCursorVisibility() {
        showCursor = !showCursor;
        Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Confined;
        Cursor.visible = showCursor ? true : false;
    }
}
