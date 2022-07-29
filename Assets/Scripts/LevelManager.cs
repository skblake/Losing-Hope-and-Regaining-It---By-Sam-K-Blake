using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    ///// INSTANTIATE IN INSPECTOR ////// 
    public enum LEVEL_ID {TITLE, FOREST, SNOWSTORM, CLEARING}
    public LEVEL_ID myID; 
    public NPCBunny npc;
    // public PlayerBunny player;
    public PlayerTrigger trigger;
    public List<Transform> newTargets;
    public AudioClip stormSound;
    public AudioSource background; 

    /////// TUNING VARIABLES ///////
    public float secsBeforeStorm = 20f;
    public float stillFaceDuration = 30f;
    


    ////// SNOWSTORM VARIABLES ///////
    private float timer = 0f; 
    private float stormDuration = 20f;

    private bool storming = false;
    private bool targetsUpdated = false;
    

    void Start()
    {
        stormDuration = stormSound.length;
        stillFaceDuration += secsBeforeStorm;
        stormDuration += stillFaceDuration;
    }

    void Update()
    { // This is not a great way of doing this and if I had more time I would just
      // have a separate manager script for each level that inherits from the same
      // base class, but since this is just a prototype it should be ok for now
        switch (myID) {

            case LEVEL_ID.FOREST:
                if (trigger != null && trigger.isHit)
                {
                    Debug.Log("MANAGER SEES TRIGGER");
                    trigger.isHit = false;
                    npc.UpdateTargetList(newTargets);
                    trigger.gameObject.SetActive(false);
                }
            break;

            case LEVEL_ID.SNOWSTORM:

                timer += Time.deltaTime;

                if (!storming && timer > secsBeforeStorm) { // START STORM
                    Debug.Log("BEGIN STORM");
                    storming = true;
                    background.clip = stormSound;
                    background.PlayOneShot(stormSound);
                    background.loop = true;
                    npc.StillFace();
                } else if (!targetsUpdated && timer > stillFaceDuration) {
                    npc.UpdateTargetList(newTargets);
                } else if (timer > stormDuration) {
                    Debug.Log("END LEVEL");
                    SceneManager.LoadSceneAsync("3 - CLEARING");
                }

            break;
        }
    }

    public void StartButtonPress()
    {

    }
}