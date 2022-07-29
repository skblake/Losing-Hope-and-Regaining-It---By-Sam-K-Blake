using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    ///// INSTANTIATE IN INSPECTOR ////// 
    public enum LEVEL_ID {TITLE, FOREST, SNOWSTORM, CLEARING}
    public LEVEL_ID myID; // This MUST be set in inspector
    public NPCBunny npc;
    public PlayerTrigger triggerInit;
    public PlayerTrigger triggerEnd;
    public List<Transform> newTargets;
    public AudioClip stormSound;
    public AudioSource background; 

    /////// TUNING VARIABLES ///////
    public float secsBeforeStorm = 20f;
    public float stillFaceDuration = 30f;
    public float forestEndSecs = 30f;
    
    private float timer = 0f; 
    private bool sceneLoaded = false;

    ////// SNOWSTORM VARIABLES ///////
    private float stormDuration = 20f;

    private bool storming = false;
    private bool targetsUpdated = false;
    

    void Start()
    {
        if (myID == LEVEL_ID.SNOWSTORM) {
            stormDuration = stormSound.length;

            stillFaceDuration += secsBeforeStorm;
            stormDuration += stillFaceDuration;
        }
    }

    void Update()
    { // This is not a great way of doing this and if I was building the foundation
      // for a large game I would just have a separate manager script for each level 
      // that inherits from the same base class, but since the scripted events in 
      // this prototype are pretty simple it should be ok for now

        switch (myID) {

            case LEVEL_ID.FOREST: ///////////// FOREST LEVEL /////////////

                if (triggerInit != null && triggerInit.isHit)
                {
                    Debug.Log("MANAGER SEES TRIGGER");
                    triggerInit.isHit = false;
                    npc.UpdateTargetList(newTargets);
                    triggerInit.gameObject.SetActive(false);
                } 
                else if (triggerEnd != null && triggerEnd.isHit)
                {
                    Debug.Log ("BEGIN END TIMER");
                    timer += Time.deltaTime; 
                    if (!sceneLoaded && timer > forestEndSecs) 
                    {
                        sceneLoaded = true;
                        Debug.Log("END LEVEL");
                        SceneManager.LoadSceneAsync("2 - SNOWSTORM");
                    }
                }
            break;

            case LEVEL_ID.SNOWSTORM: ////////// SNOWSTORM LEVEL //////////

                timer += Time.deltaTime;

                if (!storming && timer > secsBeforeStorm) // START STORM
                { 
                    Debug.Log("BEGIN STORM");
                    storming = true;
                    background.clip = stormSound;
                    background.PlayOneShot(stormSound);
                    background.loop = true;
                    npc.StillFace();
                } 
                else if (!targetsUpdated && timer > stillFaceDuration) 
                {
                    targetsUpdated = true;
                    background.PlayOneShot(stormSound);
                    npc.UpdateTargetList(newTargets);
                } 
                else if (!sceneLoaded && timer > stormDuration) 
                {
                    sceneLoaded = true;
                    background.PlayOneShot(stormSound);
                    Debug.Log("END LEVEL");
                    SceneManager.LoadSceneAsync("3 - CLEARING");
                }

            break;
        }
    }
}