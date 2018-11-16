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

    //Cameralimits currently xy (left,right,top,bottom)
    public float[] currentLimits = new float[4] { 0, 0, 0, 0 };

    private float[] LEVEL1LIMITS = new float[4] { 100, 170, 60,  50 };
    private float[] LEVEL2LIMITS = new float[4] { 100, 100, 60, 60};
    private float[] LEVEL3LIMITS = new float[4] { 100, 105, 60,  50 };
    private float[] LEVEL4LIMITS = new float[4] { 0, 500, 92, -90 };
    private float[] LEVEL5LIMITS = new float[4] { 80, 485, 60, -60 };

    private float leftLimit;
    private float rightLimit;
    private float topLimit;
    private float bottomLimit;
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
            case 0:
                currentLimits = LEVEL1LIMITS;
                break;
            case 1:
                currentLimits = LEVEL2LIMITS;
                break;
            case 2:
                currentLimits = LEVEL3LIMITS;
                break;
            case 3:
                currentLimits = LEVEL4LIMITS;
                break;
            case 4:
                currentLimits = LEVEL5LIMITS;
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
        
        if(!target) return;
	}


	void FixedUpdate()
	{
        if (playerSet)
        {
            if (useFixedUpdate)
                updateCameraPosition();
        }

	}


    //Update camera position on the player and limit the camera limits on level
	void updateCameraPosition()
	{
        if(!target) return;
        Vector3 fixedPos = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.y);
        fixedPos.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, target.position - new Vector3(0,0,20), ref _smoothDampVelocity, smoothDampTime);
       // transform.position = Vector3.Lerp(transform.position, fixedPos, Time.deltaTime * Mathf.Clamp((fixedPos - transform.position).sqrMagnitude * 8, .1f, 5));
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, currentLimits[0], currentLimits[1]), Mathf.Clamp(transform.position.y, currentLimits[3], currentLimits[2]), transform.position.z);
        originalCameraPosition = transform.position;
    }
	
}
