using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBunny : Bunny
{
    // INSTANTIATE IN INSPECTOR
    public float speed = 10f;
    public float leanDegrees = 20f;
    public float touchDist = 0.5f; // min distance to hit target
    public bool isResponsive = true;

    // For adding & ordering target destinations as empty gameobjects in scene
    public List<Transform> targets = new List<Transform>();
    private List<Vector3> _targets = new List<Vector3>();

    // Simple state machine
    [HideInInspector] public enum NPCBunnyState { Waiting, SeekingNext, Following, NoTargets, Still }
    private NPCBunnyState myState = NPCBunnyState.NoTargets;
    private Vector3 leanVector;
    private Vector3 move;

    new void Start()
    {
        UpdateTargetList(targets);

        if (isResponsive) {
            myState = NPCBunnyState.Waiting;
        } else if (_targets.Count > 0) {
            myState = NPCBunnyState.Still;
        }
        
        leanVector = new Vector3 (leanDegrees, 0f, 0f);
        
        base.Start();
    }

    new void Update() 
    {
        switch (myState) {

            case NPCBunnyState.Waiting: ///////// WAITING STATE /////////

                FacePlayer(false);

                if (DistToPartner < sightRadius) {
                    myPartner.timeSinceReply = 0f;
                    if (_targets.Count > 0) {
                        currentMood = funkyFace;
                        myState = NPCBunnyState.SeekingNext;
                    } else {
                        currentMood = happyFace;
                        myState = NPCBunnyState.Following;
                    }
                }

                if (elapsedTime > songTimer) Sing();
                SendToGround();

            break;
            
            case NPCBunnyState.SeekingNext: ///////// SEEKING STATE /////////

            Debug.Log("SEEKING TARGET " + _targets[0]);

                move = _targets[0] - transform.position;
                if (move.magnitude > touchDist) {
                    FaceTarget(true);
                    controller.Move(Vector3.Normalize(move) * speed * Time.deltaTime);
                } else {
                    Debug.Log("TARGET REACHED");
                    Sing(); 
                    FaceTarget(false);
                    _targets.Remove(_targets[0]);
                    currentMood = neutralFace;
                    myState = NPCBunnyState.Waiting;
                }
                if (DistToPartner < sightRadius) myPartner.timeSinceReply = 0f;
                SendToGround();

            break;

            case NPCBunnyState.Following: ///////// FOLLOWING STATE /////////

                myPartner.timeSinceReply = 0f; // sight will only fade when outside radius

                if (DistToPartner > sightRadius) {
                    currentMood = neutralFace;
                    myState = NPCBunnyState.Waiting;
                } else {
                    move = (myPartner.myGraphic.position - transform.position);
                    controller.Move(Vector3.Normalize(move) * speed * Time.deltaTime);
                }

                FacePlayer(DistToPartner > touchDist);

            break;

            case NPCBunnyState.Still: //////////// STILL STATE ////////////
            
                FaceTarget(false);
                move = _targets[0] - transform.position;
                if (move.magnitude > touchDist) {
                    controller.Move(move * speed * Time.deltaTime);
                } else {
                    Debug.Log("TARGET REACHED");
                    _targets.Remove(_targets[0]);
                    if (_targets.Count <= 0) myState = NPCBunnyState.NoTargets;
                }

                SendToGround();

            break;

            case NPCBunnyState.NoTargets: break;

            default:
                Debug.Log("ERROR: Unrecognized state " + myState);
            break;
        }

        base.Update();
    }

    new void Sing()
    {
        // Update volume based on distance between bunnies -- 
        // sound attenuation is linear -- for nonlinear attenuation would use 
        // lerp -- this sets the volume only one time and does not update as 
        // player's position changes, but that isn't too noticeable for short
        // sounds like these ones
        volume = 1 - (DistToPartner / maxSongDist);

        base.Sing();
    }

    new public IEnumerator SingBack ()
    {
        Debug.Log("REPLY");
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

    // Applies gravity. Gravity is not applied in all cases so NPC still
    // hops on player's head.
    void SendToGround() {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // Replaces contents of target list with new list. 
    public void UpdateTargetList(List<Transform> trans) {
        _targets = new List<Vector3>();
        foreach (Transform t in trans) {
            _targets.Add(t.position);
            Debug.Log("FOUND " + _targets.Count + " TARGETS");
        }

        if (_targets.Count > 0) myState = NPCBunnyState.SeekingNext;
    }

    void Face(Vector3 pos, bool leansToward)
    {
        transform.LookAt(pos, Vector3.up);
        
        if (leansToward) {
            transform.localEulerAngles = new Vector3 ( // lean slightly forward
                Vector3.Slerp(transform.localEulerAngles, leanVector, 1f).x, // lerp lean
                transform.localEulerAngles.y, transform.localEulerAngles.z);
        } else {
            transform.localEulerAngles = new Vector3 ( // Reset X axis rotation to 0
                0f, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
    }

    void FaceTarget(bool leansToward)
    {
        if (_targets.Count > 0) {
            Face(_targets[0], leansToward);
        }
    }

    void FacePlayer(bool leansToward) => Face(myPartner.myGraphic.position, leansToward);

}