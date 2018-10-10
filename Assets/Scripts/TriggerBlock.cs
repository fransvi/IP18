using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBlock : MonoBehaviour {


    public GameObject triggeredBlock;
    public bool hasTriggered = false;

    public void TriggerPlatformMove()
    {
        if (!hasTriggered)
        {
            Debug.Log("Triggerplatformove");
        }
        hasTriggered = true;
    }


}
