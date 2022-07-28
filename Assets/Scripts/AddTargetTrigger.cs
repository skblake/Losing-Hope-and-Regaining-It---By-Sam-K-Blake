using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AddTargetTrigger : MonoBehaviour
{
    public bool isHit = false;

    void OnTriggerEnter (Collider activator) {
        if (activator.GetComponent<PlayerBunny>() != null) {
            Debug.Log ("PLAYER ENTERED TRIGGER");
            isHit = true;
        }
    }
}