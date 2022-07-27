using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBunny : Bunny
{
    public float speed = 10f;
    public bool movesToPartner = true;

    // For adding target destinations 
    public List<Transform> targets = new List<Transform>();

    private Vector3 move;

    new void Update()
    {
        if (DistToPartner < sightRadius) {
            myPartner.LogReply(); // Sight will not fade if partner is in range

            if (movesToPartner) {
                // Calculate vector from NPC position to player position
                move = (myPartner.myGraphic.position - transform.position);

                // moves player in correct horizontal direction at the correct speed
                controller.Move(move * speed * Time.deltaTime);
            }
        }   
        if (elapsedTime > songTimer) Sing();

        base.Update();
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

    // TODO: Enable cinematics by passing location in here
    public IEnumerator MoveToTarget(Vector3 pos, float yRotation)
    {
        yield return null;
    }
}