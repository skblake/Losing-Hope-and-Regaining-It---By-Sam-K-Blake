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
    private float jumpHeight = 1f;
    private bool grounded;
    public float overlayDelaySecs = 3f;

    ////// MOVEMENT VARIABLES ////// 
    private float x = 0f; 
    private float z = 0f;
    private Vector3 move;
    public bool isMoving;

    ////// COLOR VARIABLES ////// 
    public float colorLerpSecs = 3f;
    public float blackoutDist = 5f;
    public float blackoutSecs = 7f;
    private Color targetColor;
    private Material waterOverlay;
    private float currEffectStrength = 0;

    new void Start() 
    {
        base.Start();
        waterOverlay = sightOverlay.material;
        waterOverlay.SetFloat("_normalIntensity", 0);
        waterOverlay.SetFloat("_displacementIntensity", 0);
    }

    new void Update() 
    {
        ///////////////// MOVEMENT /////////////////
        
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

        velocity.y += gravity * Time.deltaTime; // add gravity to y axis movement

        controller.Move(velocity * Time.deltaTime); // Move player vertically

        ///////////////// SINGING /////////////////
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) {
            Sing(); // Sing if 'E' or left mouse are pressed
        }

        /////////// UPDATE SIGHT OVERLAY ///////////
        timeSinceReply += Time.deltaTime;
        UpdateSightOverlay();

        base.Update();
    }

    void UpdateSightOverlay()
    {
        if (DistToPartner > sightRadius && timeSinceReply > overlayDelaySecs) {
            currEffectStrength = (timeSinceReply - overlayDelaySecs) / blackoutSecs;
        } else {
            currEffectStrength = 0;
        }

        currEffectStrength /= 2;

        waterOverlay.SetFloat("_normalIntensity", currEffectStrength);
        waterOverlay.SetFloat("_displacementIntensity", currEffectStrength);
    }
}