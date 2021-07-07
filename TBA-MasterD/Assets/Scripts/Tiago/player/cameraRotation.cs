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
    void Start() {
        Debug.LogWarning("To Hide the mouse cursor just press 'K'");
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

    private void ToggleCursorVisibility() {
        showCursor = !showCursor;
        Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Confined;
        Cursor.visible = showCursor ? true : false;
    }
}
