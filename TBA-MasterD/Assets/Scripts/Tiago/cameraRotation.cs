using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotation : MonoBehaviour
{
    //movimentos do Rato
    private float mouseX;
    private float mouseY;
    public float mouseSensitivy = 100f;

    //corpo do jogador
    public Transform playerBody;
    private float xRotation = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }


    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivy * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivy * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);

    }
}
