using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PURPOSE: Player movement and jump.
//   USAGE: Attach to player with Character Controller component and child graphics object.

public class Movement : MonoBehaviour

{
    public CharacterController controller; 
    public float speed = 10f;

    ////// PHYSICS VARIABLES //////
    private float gravity = -9.81f; // based on Earth's gravity
    private Vector3 velocity;
    private float jumpHeight = 1f;
    private bool grounded;

    ////// MOVEMENT VARIABLES ////// 
    private float x = 0f; 
    private float z = 0f;
    private Vector3 move;

    void Start() 
    {
        controller = this.GetComponent<CharacterController>();
    }

    void Update() 
    {
        grounded = controller.isGrounded;

        if (grounded && velocity.y < 0) // Zeroes velocity when player reaches
            velocity.y = 0;             // the ground
        
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        move = transform.right * x + transform.forward * z;

        // moves player in correct horizontal direction at the correct speed
        controller.Move(move * speed * Time.deltaTime);

        Debug.Log(grounded);
        if (Input.GetButtonDown("Jump") && grounded)
        {
            Debug.Log("JUMP");
            // Changes y axis movement by:
            // square root of jump height * current direction * gravity.
            velocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }

        // adds gravity to y axis movement
        velocity.y += gravity * Time.deltaTime;

        // Moves player vertically
        controller.Move(velocity * Time.deltaTime);

    }
}