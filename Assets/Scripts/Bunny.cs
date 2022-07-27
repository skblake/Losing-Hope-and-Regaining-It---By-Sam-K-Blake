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
    public SongManager manager;
    public CharacterController controller; 
    public Transform myGraphic;

    // faces
    public SkinnedMeshRenderer myRenderer; // renderer should have two materials
    public Material singFace;              // (first is body and second is face)
    public Material neutralFace; 

    ////// INSTANTIATED AT RUNTIME ////// 
    public Bunny myPartner;
    //public bool automated = false;
    public string PartnerName { get { return myPartner.gameObject.name; }}

    ////////// AUDIO VARIABLES ////////// 
    protected float volume = 1f;
    protected float elapsedTime = 0f;

    //////////// PROPERTIES /////////////
    public float TimeSinceSing {
        get { return elapsedTime / songTimer; } 
        set { elapsedTime = value; }
        }
    
    public float DistToPartner {
        get { return Vector3.Distance(transform.position, myPartner.transform.position); }
    }
    
    public float timeSinceReply = 0f;
    private bool isSinging; 
    
    protected void Start() 
    {
        controller = this.GetComponent<CharacterController>();
        myPartner = GameObject.FindWithTag("Bunny").GetComponent<Bunny>();
        myPartner.myPartner = this; // lol there is definitely a better way to do this - 
                                    // bugfix: one bunny will always set itself as its own partner
        Debug.Log(gameObject.name + ": My partner is " + PartnerName + " and their partner is " + myPartner.PartnerName);
    }

    protected void Update() 
    {
        elapsedTime += Time.deltaTime; 
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
        StartCoroutine("UpdateFace", mySong.clip.length);
    }

    // Allows player and NPC to behave differently when their partner sings
    public IEnumerator SingBack () 
    { 
        myPartner.LogReply();
        yield return null; 
    }

    // displays singing face while sound is playing. duration = length in seconds.
    IEnumerator UpdateFace(float duration)
    {
        Material[] mats = myRenderer.materials;
        mats[1] = singFace;
        myRenderer.materials = mats;

        float singTime = 0f;

        while (singTime < duration) {
            singTime += Time.deltaTime;
            yield return null;
        }

        mats[1] = neutralFace;
        myRenderer.materials = mats;
        yield return null;
    }

    public void LogReply() => timeSinceReply = 0f;
}