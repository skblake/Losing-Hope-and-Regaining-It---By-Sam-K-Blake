using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PURPOSE: A graveyard for unused code in case I need to dig it up again.

public class CodeGraveyard : MonoBehaviour
{
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////// IN MEMORIAM : NPCBUNNY CODE 2022-2022 //////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////// 
    /*

    // This code is all from before I caved and just gave NPCBunny a state machine.

    void OldUpdate()
    {
        // Debug.Log("Targets: " + targets.Count);

        if (targetFeature) {
            UpdateTargets();
        } else {
            if (DistToPartner < sightRadius) {
            myPartner.LogReply(); // Sight will not fade if partner is in range

                if (isResponsive) {
                    // Calculate vector from NPC position to player position
                    move = (myPartner.myGraphic.position - transform.position);

                    // moves player in correct horizontal direction at the correct speed
                    controller.Move(move * speed * Time.deltaTime);

                    // rotates to face player
                    transform.LookAt(myPartner.myGraphic.position, Vector3.up);
                    transform.localEulerAngles = new Vector3( // Reset X axis rotation to 0
                        0f,
                        transform.localEulerAngles.y,
                        transform.localEulerAngles.z
                    );
                }    
            }
        }

       
        if (elapsedTime > songTimer) Sing();

        base.Update();
    }

    void UpdateTargets()
    {
         if (isResponsive) {
            if (DistToPartner < sightRadius && !seesPartner) {
                Debug.Log("Insert point 1");
                _targets.Insert(0, myPartner.transform.position);
                seesPartner = true;
            } else if (DistToPartner > sightRadius) {
                Debug.Log("Remove point 1");
                _targets.Remove(myPartner.transform.position);
                seesPartner = false;
                Debug.Log("Targets: " + _targets.Count);
            }
        }

        if (_targets.Count > 0) {
            if (seekNext) {
                // Moves player
                move = _targets[0] - transform.position; // Calculate vector
                moveResult = controller.Move(move * speed * Time.deltaTime);

                Debug.Log("moveResult: " + moveResult);
                // move player and store collision info

                if (moveResult != CollisionFlags.None || moveResult != CollisionFlags.Below) { // it hit something
                    if (move.magnitude < touchDist) { // target reached
                        StartCoroutine("AwaitPlayer"); // Queue target pop
                    } else { // just sends kinda backwards if it hits random object
                        Debug.Log("Insert point 3");
                        _targets.Insert(0, -transform.forward + -transform.right);
                    }
                }
            }

            transform.LookAt(_targets[0], Vector3.up);
            transform.localEulerAngles = new Vector3( // Reset X axis rotation to 0
                0f,
                transform.localEulerAngles.y,
                transform.localEulerAngles.z
            );
        }
    }

    // Updates target only once NPC is in sight of player
    public IEnumerator AwaitPlayer()
    {
        Debug.Log("AWAITING PLAYER");
        seekNext = false;

        // NPC will look at player while waiting for them.
        Debug.Log("Coroutine insert point");
        _targets.Insert(0, myPartner.transform.position); 

        while (!seesPartner) yield return null;

        Debug.Log("Remove point 2");
        Debug.Log("Remove point 3");
        targets.Remove(targets[0]); // pop first item from list
        targets.Remove(targets[0]); // twice (we added player)
        seekNext = true;

        
        yield return null;
    }

    // TODO: Enable cinematics by passing location in here
    public IEnumerator MoveToTarget(Vector3 pos, float yRotation)
    {
        yield return null;
    }

    public static float DegreesToRadians(float degrees) { return (Mathf.PI / 180) * degrees; }

    public static float RadiansToDegrees(float radians) { return (180 / Mathf.PI) * radians; }
    */

    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////


    //                              ~* GONE BUT NOT FORGOTTEN *~                                     //


    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////// IN MEMORIAM : PLAYERBUNNY CODE 2022-2022 /////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////// 

    // Makes sight overlay a flat color. I'll probably use this to generate a backup build for
    // accessibility. Goes with this line in Start(): 

    //         targetColor = sightOverlay.color;

    /*
        void OldUpdateSightOverlay() 
    {
        /////////// UPDATE SIGHT OVERLAY ///////////
        timeSinceReply += Time.deltaTime;

        if (DistToPartner > sightRadius) { // player is outside range
            targetColor.a = timeSinceReply / blackoutSecs;
        } else {
            targetColor.a = 0f;
        }

        sightOverlay.color = Color.Lerp(sightOverlay.color, targetColor, colorLerpSecs);
        
        // targetColor.a = DistToPartner / blackoutDist + (timeSinceReply / blackoutSecs);
    }
    */

    // He doesn't need to jump - First code block goes at the beginning of update and the 
    // if {} goes right before y velocity updates

    /*
            grounded = controller.isGrounded;
        
        // if (grounded) Debug.Log("GROUNDED");

        if (grounded && velocity.y < 0) // Zeroes velocity when player reaches
            velocity.y = 0;             // the ground
    */

    /*
            if (Input.GetButtonDown("Jump") && grounded)
        {
            Debug.Log("JUMP");
            // Changes y axis movement by:
            // square root of jump height * current direction * gravity.
            velocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    */

    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////


    //                              ~* GONE BUT NOT FORGOTTEN *~                                     //


    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////// IN MEMORIAM : MOUSELOOK CODE 2022-2022 //////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////// 

    // And may it rest in peace.

    /* 

            // bunnyScript = playerBody.GetComponent<PlayerBunny>();

    /*

    void TwoAxisCamera() 
    {
        // Find mouse position and adjust by sensitivity and frame rate
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY; // decreases xRotation based on mouse position
        yRotation += mouseX / 2; 
        
        // stops player from looking all the way above/below them
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // apply rotation to camera
        
        // bunnyScript.GetComponent<Transform>().localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // bunnyScript.GetComponent<Transform>().Rotate = (Vector3.up * mouseX);
        // bunnyScript.GetComponent<Transform>().localRotation = Quaternion.Euler(0f, yRotation, 0f);

        // apply rotation to player
        
        // bunnyScript.GetComponent<Transform>().Rotate(Vector3.right * mouseY); 

        // float z = pivot.eulerAngles.z;
        

        if (bunnyScript.isMoving) {
            // apply rotation to player graphic
            pivot.transform.localEulerAngles = new Vector3( // Reset X axis rotation to 0
                    0f,
                    0f,
                    0f
            );

            /*
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            
            bunnyScript.GetComponent<Transform>().Rotate(Vector3.up * mouseX);
            playerBody.localRotation = Quaternion.Euler(0f, yRotation, 0f);            
            playerBody.Rotate(Vector3.up * mouseX);*/ /*
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY; // decreases xRotation based on mouse position
            
            // stops player from looking all the way above/below them
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // apply rotation to camera
            pivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // apply rotation to player
            playerBody.Rotate(Vector3.up * mouseX); 
            

        } else {

            pivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            pivot.Rotate(Vector3.up * mouseX);
            // pivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        // apply rotation to camera
        // transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // pivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // apply rotation to player
        // playerBody.Rotate(Vector3.up * mouseX); 




        /*
                // apply rotation to camera
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        bunnyScript.GetComponent<Transform>().rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        pivot.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        */ /*
    }

    public Transform m_GameManager;
    
    public float xSpeed = 20.0f;
    public float ySpeed = 80.0f;

    public float yMinLimit = -80f;
    public float yMaxLimit = 80f;

    private float distance = 5f; //needs to be based off of game size (how many cubes), not random number.
    private float distanceMin = 4f;
    private float distanceMax = 15f;

    private new Rigidbody rigidbody;
    private Quaternion prevRot;
    private float prevDistance;

    float x = 0.0f;
    float y = 0.0f;

    void TempGabeCamera() 
    {
        if (Vector3.Distance(this.transform.position, m_GameManager.position) <= distance)
        {
            CameraTranslateOut(); //while the camera is inside the game area, move it backwards until it reaches the default distance. 
        }        

        Quaternion rotation = this.transform.rotation;

        if (m_GameManager && (Input.GetButton("Fire2")))
        {
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            rotation = Quaternion.Euler(y, x, 0);

        }
        
        distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

        if(distance != prevDistance || rotation != prevRot) //if distance or rotation has changed, then run set pos/rot
        {
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + m_GameManager.position;

            transform.rotation = rotation;
            transform.position = position;

            prevRot = rotation;
            prevDistance = distance;
        }
    }

    public static float ClampAngle(float angle, float min, float max) //restricts an angle based off of a minimum and maximum. prevents angle from going over 360 or under 0.
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    private void CameraTranslateOut()
    {
        for (int i = 0; i < 300; i++)
        {
            this.transform.Translate((Vector3.forward * -1) * (Time.deltaTime * 0.02f)); //Slowly pans backwards
        }        
    }*/

    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////


    //                              ~* GONE BUT NOT FORGOTTEN *~                                     //


}
