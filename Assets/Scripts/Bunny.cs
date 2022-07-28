using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PURPOSE: Basic bunny behaviors like singing and searching for other bunnies.
//   USAGE: Inherit from this class for specific bunny types.

public class Bunny : MonoBehaviour
{

    ////////// TUNING VARIABLES //////////
    public float maxSongDist = 200f;
    public float songTimer = 10f;
    public float replyOffset = 2f; // response delay in seconds
    public float replyVariability = 0.5f; // variability in %
    public float sightRadius = 2.5f;

    ////// INSTANTIATE IN INSPECTOR //////
    public AudioSource mySong;
    public LevelManager manager;
    public CharacterController controller; 
    public Transform myGraphic;
    public Bunny myPartner;

    // faces
    public SkinnedMeshRenderer myRenderer; // renderer should have two materials
    public Material singFace;              // (1 - body and 2 - face)
    public Material neutralFace; 
    public Material happyFace;
    public Material funkyFace;
    protected Material currentMood;
    private Material prevMood;
    private Material[] mats; 

    ////// INSTANTIATED AT RUNTIME ////// 
    //public bool automated = false;
    public string PartnerName { get { return myPartner.gameObject.name; }}

    ////////// AUDIO VARIABLES ////////// 
    protected float volume = 1f;
    protected float elapsedTime = 0f;

    /////////// SHARED PHYSICS VARIABLES ///////// 

    protected float gravity = -9.81f; // based on Earth's gravity
    protected Vector3 velocity;

    //////////// PROPERTIES /////////////
    public float TimeSinceSing {
        get { return elapsedTime / songTimer; } 
        set { elapsedTime = value; }
        }
    
    public float DistToPartner {
        get { return Vector3.Magnitude(myPartner.transform.position - transform.position); }
    }
    
    public float timeSinceReply = 0f;
    private bool isSinging; 
    
    protected void Start() 
    {
        controller = this.GetComponent<CharacterController>();
        mats = myRenderer.materials;
        currentMood = neutralFace;
    }

    protected void Update() 
    {
        elapsedTime += Time.deltaTime; 

        // Update face
        if (isSinging) {
            SetFace(singFace);
        } else {
            SetFace(currentMood);
        }

    }

    // play sound 
    public void Sing() 
    {
        // Debug.Log(gameObject.name + " SING AT VOL " + volume);

        if (volume > 0) { 
            mySong.PlayOneShot(mySong.clip, volume);
            myPartner.StartCoroutine("SingBack");
        }

        elapsedTime = 0f; // Reset timer even if no sound plays
        StartCoroutine("UpdateSingFace", mySong.clip.length);
    }

    // Allows player and NPC to behave differently when their partner sings
    public IEnumerator SingBack () 
    { 
        myPartner.LogReply();
        yield return null; 
    }

    // displays singing face while sound is playing. duration = length in seconds.
    IEnumerator UpdateSingFace(float duration)
    {
        isSinging = true;
        float singTime = 0f;

        while (singTime < duration) {
            singTime += Time.deltaTime;
            yield return null;
        }

        isSinging = false;
        yield return null;
    }

    public void LogReply() => timeSinceReply = 0f;

    // Helper function for child classes to change bunny's face
    protected void SetFace(Material m) {
        mats[1] = m;
        myRenderer.materials = mats;
    } 
}