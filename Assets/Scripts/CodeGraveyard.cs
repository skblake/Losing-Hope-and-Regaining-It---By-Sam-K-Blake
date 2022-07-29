using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PURPOSE: A graveyard for unused code in case I need to dig it up again.

public class CodeGraveyard : MonoBehaviour
{
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////// IN MEMORIUM : NPCBUNNY CODE 2022-2022 //////////////////////////////
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
    //////////////////////////// IN MEMORIUM : PLAYERBUNNY CODE 2022-2022 /////////////////////////////
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

    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////


    //                              ~* GONE BUT NOT FORGOTTEN *~                                     //


}
