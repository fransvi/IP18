using UnityEngine;
using System.Collections;
using Char2DController;


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

    public float leftLimit;
    public float rightLimit;
    public float topLimit;
    public float bottomLimit;
    Vector3 originalCameraPosition;

    private CharacterController2D _playerController;
	private Vector3 _smoothDampVelocity;
	
    //Set active player prefab for the camera
    public void SetPlayer(GameObject player)
    {
        target = player.transform;
        transform = gameObject.transform;
        _playerController = target.GetComponent<CharacterController2D>();
        playerSet = true;
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
