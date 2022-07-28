using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBunny : Bunny
{
    public float speed = 10f;
    public bool targetFeature;
    public float touchDist = 0.5f; // min distance to hit target
    public bool movesToPartner = true;

    // For adding & ordering target destinations as empty gameobjects in scene
    public List<Transform> guideTargets = new List<Transform>();
    private List<Vector3> targets = new List<Vector3>();

    private Vector3 move;
    private bool seesPartner = false;
    private bool seekNext = true;
    private CollisionFlags moveResult;

    new void Start()
    {
        foreach (Transform t in guideTargets) {
            targets.Add(t.position);
        }

        base.Start();
    }

    new void Update()
    {
        // Debug.Log("Targets: " + targets.Count);

        if (targetFeature) {
            UpdateTargets();
        } else {
            if (DistToPartner < sightRadius) {
            myPartner.LogReply(); // Sight will not fade if partner is in range

                if (movesToPartner) {
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
         if (movesToPartner) {
            if (DistToPartner < sightRadius && !seesPartner) {
                Debug.Log("Insert point 1");
                targets.Insert(0, myPartner.transform.position);
                seesPartner = true;
            } else if (DistToPartner > sightRadius) {
                Debug.Log("Remove point 1");
                targets.Remove(myPartner.transform.position);
                seesPartner = false;
                Debug.Log("Targets: " + targets.Count);
            }
        }

        if (targets.Count > 0) {
            if (seekNext) {
                // Moves player
                move = targets[0] - transform.position; // Calculate vector
                moveResult = controller.Move(move * speed * Time.deltaTime);

                Debug.Log("moveResult: " + moveResult);
                // move player and store collision info

                if (moveResult != CollisionFlags.None || moveResult != CollisionFlags.Below) { // it hit something
                    if (move.magnitude < touchDist) { // target reached
                        StartCoroutine("AwaitPlayer"); // Queue target pop
                    } else { // just sends kinda backwards if it hits random object
                        Debug.Log("Insert point 3");
                        targets.Insert(0, -transform.forward + -transform.right);
                    }
                }
            }

            transform.LookAt(targets[0], Vector3.up);
            transform.localEulerAngles = new Vector3( // Reset X axis rotation to 0
                0f,
                transform.localEulerAngles.y,
                transform.localEulerAngles.z
            );
        }
    }

    new void Sing()
    {
        // Update volume based on distance between bunnies -- 
        // sound attenuation is linear -- for nonlinear attenuation would use 
        // lerp -- this sets the volume only one time and does not update as 
        // player's position changes, but that is not too noticeable for short
        // sounds like these ones
        volume = 1 - (DistToPartner / maxSongDist);

        base.Sing();
    }

    new public IEnumerator SingBack ()
    {
        float delayTime = Time.deltaTime;

        // Randomizing response time makes NPC more lifelike
        float randomDelay = Random.Range(
            replyOffset * (1 - replyVariability), 
            replyOffset * (1 + replyVariability)
        );

        while (delayTime < randomDelay) {
            delayTime += Time.deltaTime;
            yield return null;
        }

        myPartner.LogReply();
        Sing();
        yield return null;
    }

    // Updates target only once NPC is in sight of player
    public IEnumerator AwaitPlayer()
    {
        Debug.Log("AWAITING PLAYER");
        seekNext = false;

        // NPC will look at player while waiting for them.
        Debug.Log("Coroutine insert point");
        targets.Insert(0, myPartner.transform.position); 

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
}