using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// PURPOSE: A bunny that can move, jump, and sing with a button press. 
//   USAGE: Attach to player with Character Controller component and child graphics object.

public class PlayerBunny : Bunny

{
    //////// INSTANTIATE IN INSPECTOR //////
    public Image sightOverlay;

    ////// PHYSICS VARIABLES //////
    public float speed = 10f;
    private float gravity = -9.81f; // based on Earth's gravity
    private Vector3 velocity;
    private float jumpHeight = 1f;
    private bool grounded;

    ////// MOVEMENT VARIABLES ////// 
    private float x = 0f; 
    private float z = 0f;
    private Vector3 move;
    public bool isMoving;

    ////// COLOR VARIABLES ////// 
    public float colorLerpSecs = 3f;
    public float blackoutSecs = 7f;
    private Color targetColor;

    new void Start() 
    {
        targetColor = sightOverlay.color;
        base.Start();
    }

    new void Update() 
    {
        ///////////////// MOVEMENT /////////////////
        grounded = controller.isGrounded;
        
        // if (grounded) Debug.Log("GROUNDED");

        if (grounded && velocity.y < 0) // Zeroes velocity when player reaches
            velocity.y = 0;             // the ground
        
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        move = transform.right * x + transform.forward * z;

        if (move != Vector3.zero) {
            // Debug.Log("MOVING");
            isMoving = true;
        } else {
            // Debug.Log("NOT MOVING");
            isMoving = false;
        }

        // moves player in correct horizontal direction at the correct speed
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            Debug.Log("JUMP");
            // Changes y axis movement by:
            // square root of jump height * current direction * gravity.
            velocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }

        velocity.y += gravity * Time.deltaTime; // add gravity to y axis movement

        controller.Move(velocity * Time.deltaTime); // Move player vertically

        ///////////////// SINGING /////////////////
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) {
            Sing(); // Sing if 'E' or left mouse are pressed
        }

        /////////// UPDATE SIGHT OVERLAY ///////////
        timeSinceReply += Time.deltaTime;
        targetColor.a = timeSinceReply / blackoutSecs;
        sightOverlay.color = Color.Lerp(sightOverlay.color, targetColor, colorLerpSecs);

        base.Update();
    }

}