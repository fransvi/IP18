using UnityEngine;
using System.Collections;



public class SmoothFollow : MonoBehaviour
{
	public Transform target;
	public float smoothDampTime = 0.2f;
	[HideInInspector]
	public new Transform transform;
	public Vector3 cameraOffset;
	public bool useFixedUpdate = false;
    public bool playerSet = false;

    // variables used to limit camera movement point 

        private float[] LEVEL1LIMITS = new float[4] { 100, 285, 60,  50 };
        private float[] LEVEL2LIMITS = new float[4] { 100, 100, 60, -55 };
        private float[] LEVEL3LIMITS = new float[4] { 100, 450, 60,  50 };
        private float[] LEVEL4LIMITS = new float[4] { 120, 450, 60, -55 };

    public float leftLimit;
    public float rightLimit;
    public float topLimit;
    public float bottomLimit;
    Vector3 originalCameraPosition;

	private Vector3 _smoothDampVelocity;
	
    //Set active player prefab for the camera
    public void SetPlayer(GameObject player)
    {
        target = player.transform;
        transform = gameObject.transform;
        playerSet = true;
    }

    public void SetCameraLimits(int i)
    {

        switch (i)
        {

            case 1:
                leftLimit = LEVEL1LIMITS[0];
                rightLimit = LEVEL1LIMITS[1];
                topLimit = LEVEL1LIMITS[2];
                bottomLimit = LEVEL1LIMITS[3];
                break;
            case 2:
                leftLimit = LEVEL2LIMITS[0];
                rightLimit = LEVEL2LIMITS[1];
                topLimit = LEVEL2LIMITS[2];
                bottomLimit = LEVEL2LIMITS[3];
                break;
            case 3:
                leftLimit = LEVEL3LIMITS[0];
                rightLimit = LEVEL3LIMITS[1];
                topLimit = LEVEL3LIMITS[2];
                bottomLimit = LEVEL3LIMITS[3];
                break;
            case 4:
                leftLimit = LEVEL4LIMITS[0];
                rightLimit = LEVEL4LIMITS[1];
                topLimit = LEVEL4LIMITS[2];
                bottomLimit = LEVEL4LIMITS[3];
                break;
            default:
                break;
        }

    }
	
	
	void LateUpdate()
	{
        if (playerSet)
        {
            if (!useFixedUpdate)
                updateCameraPosition();
        }
	}


	void FixedUpdate()
	{
        if (playerSet)
        {
            if (useFixedUpdate)
                updateCameraPosition();
        }

	}


    //Update camera position on the player
	void updateCameraPosition()
	{
        Vector3 fixedPos = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.y);
        fixedPos.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, target.position - new Vector3(0,0,20), ref _smoothDampVelocity, smoothDampTime);
       // transform.position = Vector3.Lerp(transform.position, fixedPos, Time.deltaTime * Mathf.Clamp((fixedPos - transform.position).sqrMagnitude * 8, .1f, 5));
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftLimit, rightLimit), Mathf.Clamp(transform.position.y, bottomLimit, topLimit), transform.position.z);
        originalCameraPosition = transform.position;
    }
	
}
