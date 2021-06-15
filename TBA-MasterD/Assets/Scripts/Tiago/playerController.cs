using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    //velocidades de movimento
    public int moveSpeed;
    public int sprintSpeed;
    public int crouchSpeed;
    Vector3 velocity;
    public float gravity;
    public bool isRunning = false;
    public bool isCrouched = false;

    //extras
    public CharacterController controller;
    public Camera cam;
    public float crouchHeight;

    //Info Sobre Player
    public int vida = 100;


    //saltar
    private bool isGrounded = true;
    public float jumpHeight;


    void Update()
    {
        //andar
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (isRunning == false && isCrouched == false)
        {
            controller.Move(move * moveSpeed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        //correr
        if (Input.GetKey(KeyCode.LeftShift) && isCrouched == false)
        {
            controller.Move(move * sprintSpeed * Time.deltaTime);
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        //saltar
        if (Input.GetKeyDown("space") && isGrounded == true)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            isGrounded = false;
            StartCoroutine(jumpingCoolDown());
        }
        IEnumerator jumpingCoolDown()
        {
            yield return new WaitForSeconds(0.5f);
            isGrounded = true;
        }

        //crouch
       
        if(Input.GetKey(KeyCode.LeftControl) && isRunning == false)
        {
            controller.Move(move * crouchSpeed * Time.deltaTime);
            cam.transform.position = new Vector3(transform.position.x, transform.position.y - crouchHeight, transform.position.z);
            isCrouched = true;
            
        }
        else
        {
            cam.transform.position = new Vector3(transform.position.x, transform.position.y + crouchHeight, transform.position.z);
            isCrouched = false;
        }

        //Pause
        if (Input.GetKeyDown("escape"))
        {
            Debug.Log("Game is Paused");
        }
       
    }
}
