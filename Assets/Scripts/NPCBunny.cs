using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBunny : Bunny
{
    // INSTANTIATE IN INSPECTOR
    public float speed = 10f;
    public float touchDist = 0.5f; // min distance to hit target
    public bool isResponsive = true;

    // For adding & ordering target destinations as empty gameobjects in scene
    public List<Transform> targets = new List<Transform>();
    private List<Vector3> _targets = new List<Vector3>();

    // Simple state machine
    [HideInInspector] public enum NPCBunnyState { Waiting, SeekingNext, Following, NoTargets, Still }
    private NPCBunnyState myState = NPCBunnyState.NoTargets;
    private Vector3 move;

    new void Start()
    {
        foreach (Transform t in targets) {
            _targets.Add(t.position);
            Debug.Log("FOUND " + _targets.Count + " TARGETS");
        }

        if (isResponsive) {
            myState = NPCBunnyState.Waiting;
        } else if (_targets.Count > 0) {
            myState = NPCBunnyState.Still;
        }
        
        base.Start();
    }

    new void Update() 
    {
        switch (myState) {

            case NPCBunnyState.Waiting: ///////// WAITING STATE /////////

                if (DistToPartner < sightRadius) {
                    myPartner.LogReply(); // Sight will not fade within range

                    if (_targets.Count > 0) {
                        myState = NPCBunnyState.SeekingNext;
                    } else {
                        myState = NPCBunnyState.Following;
                    }
                }
                FacePlayer();

                if (elapsedTime > songTimer) Sing();

            break;
            
            case NPCBunnyState.SeekingNext:

                FaceTarget();
                move = Vector3.Normalize(_targets[0] - transform.position);
                if (move.magnitude > touchDist) {
                    controller.Move(move * speed * Time.deltaTime);
                } else {
                    Debug.Log("TARGET REACHED");
                    _targets.Remove(_targets[0]);
                    myState = NPCBunnyState.Waiting;
                }

            break;

            case NPCBunnyState.Following:

                if (DistToPartner > sightRadius) {
                    myState = NPCBunnyState.Waiting;
                } else {
                    move = (myPartner.myGraphic.position - transform.position);
                    controller.Move(move * speed * Time.deltaTime);
                }
                FacePlayer();

            break;

            case NPCBunnyState.Still:
            
                FaceTarget();
                move = _targets[0] - transform.position;
                if (move.magnitude > touchDist) {
                    controller.Move(move * speed * Time.deltaTime);
                } else {
                    Debug.Log("TARGET REACHED");
                    _targets.Remove(_targets[0]);
                    if (_targets.Count <= 0) myState = NPCBunnyState.NoTargets;
                }
                
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

    void Face(Vector3 pos)
    {
        transform.LookAt(pos, Vector3.up);
        transform.localEulerAngles = new Vector3( // Reset X axis rotation to 0
            0f, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    void FaceTarget()
    {
        if (_targets.Count > 0) {
            Face(_targets[0]);
        }
    }

    void FacePlayer() => Face(myPartner.myGraphic.position);

}