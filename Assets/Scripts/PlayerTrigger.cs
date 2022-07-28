using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerTrigger : MonoBehaviour
{
    public bool isHit = false;

    void OnTriggerEnter (Collider activator) {
        if (activator.GetComponent<PlayerBunny>() != null) {
            Debug.Log ("PLAYER ENTERED TRIGGER");
            isHit = true;
        }
    }
}