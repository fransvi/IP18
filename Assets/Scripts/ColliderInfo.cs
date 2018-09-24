using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderInfo : MonoBehaviour {


     //Collider types for prefabs
     //0 = End of stage block
     //1 = Instant death block
     //2 = Trap block
     //3 = Activation zone
    
    public int colliderType;
    // For moving platforms / traps
    public float moveSpeed;
    public float activationTime;
    public Transform currentPoint;
    public Transform[] movePoints;
    public int pointSelected = 0;
    public bool _platformMoving;

    public GameObject triggeredBlock;
    public bool hasTriggered = false;


    // Use this for initialization
    void Awake () {
        if(colliderType == 2)
        {
            currentPoint = movePoints[pointSelected];
        }
 
    }
	
	// Update is called once per frame
	void Update () {
        if (colliderType == 2)
        {
            if (_platformMoving)
            {
                MovePlatformBetweenPoints();
            }
        }

        
		
	}

    // trigger the platform movement once
    public void TriggerPlatformMove()
    {
        if (!hasTriggered)
        {
            triggeredBlock.GetComponent<ColliderInfo>()._platformMoving = true;
        }
        hasTriggered = true;
    }

    // move the platform between two or more points set in the array
    void MovePlatformBetweenPoints()
    {
            transform.position = Vector3.MoveTowards(transform.position, currentPoint.position, Time.deltaTime * moveSpeed);

            if (transform.position == currentPoint.position)
            {

                pointSelected++;
                if (pointSelected == movePoints.Length)
                {
                    pointSelected = 0;
                _platformMoving = false;
                }

                currentPoint = movePoints[pointSelected];
            }
        }

}
