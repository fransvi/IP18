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
		if( _playerController == null )
		{
			transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
			return;
		}
		
		if( _playerController.velocity.x > 0 )
		{
			transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
		}
		else
		{
			var leftOffset = cameraOffset;
			leftOffset.x *= -1;
			transform.position = Vector3.SmoothDamp( transform.position, target.position - leftOffset, ref _smoothDampVelocity, smoothDampTime );
		}
	}
	
}
