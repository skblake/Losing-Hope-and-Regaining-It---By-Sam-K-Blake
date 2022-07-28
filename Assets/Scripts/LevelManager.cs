using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public NPCBunny npc;
    // public PlayerBunny player;
    public PlayerTrigger trigger;
    public List<Transform> triggerTargets;

    void Update()
    {
        if (trigger.isHit)
        {
            trigger.isHit = false;
            npc.UpdateTargetList(triggerTargets);
            trigger.gameObject.SetActive(false);
        }
    }
}