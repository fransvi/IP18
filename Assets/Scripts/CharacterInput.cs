using UnityEngine;
using System.Collections;
using Char2DController;


public class CharacterInput : MonoBehaviour
{

	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;
    public float jumpHoldAddition = 1.5f;
    public float jumpHoldDuration = 2f;
    [HideInInspector]
	private float normalizedHorizontalSpeed = 0;
	private CharacterController2D _controller;
	private RaycastHit2D _lastControllerColliderHit;
    private GameController _gameController;
	private Vector3 _velocity;
    private bool _ableToJump;
    private bool _jumpKeyHold;
    private Animator _animator;

    void Awake()
	{
		_controller = GetComponent<CharacterController2D>();
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
        _animator = GetComponent<Animator>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
	}



	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;

	}



    // When hitting trigger zones, either proceed to next level or respawn the player
	void onTriggerEnterEvent( Collider2D col )
	{
		//Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
        if(col.gameObject.GetComponent<ColliderInfo>().colliderType == 0)
        {
            Debug.Log("Next stage");
            Destroy(this.gameObject);
            _gameController.NextStage();
        }
        if(col.gameObject.GetComponent<ColliderInfo>().colliderType == 1 || col.gameObject.GetComponent<ColliderInfo>().colliderType == 2)
        {
            Debug.Log("Reset stage");
            Destroy(this.gameObject);
            _gameController.ReloadStage();
        }
        if(col.gameObject.GetComponent<ColliderInfo>().colliderType == 3)
        {
            col.gameObject.GetComponent<ColliderInfo>().TriggerPlatformMove();
        }
	}


	void onTriggerExitEvent( Collider2D col )
	{
		Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}

    void EndJumpHold()
    {
        _jumpKeyHold = false;
    }



	void Update()
	{
        if( _controller.isGrounded ) {
			_velocity.y = 0;
            if (_animator.GetBool("jump")){
                _animator.SetBool("jump", false);
            }
        }
        else {
            _animator.SetBool("walk", false);
        }

        if ( Input.GetKey( KeyCode.RightArrow ) || Input.GetKey(KeyCode.D) )
		{
			normalizedHorizontalSpeed = 1;
			if( transform.localScale.x < 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			if( _controller.isGrounded)
            {
                _animator.SetBool("walk", true);
            }
		}
		else if( Input.GetKey( KeyCode.LeftArrow ) || Input.GetKey(KeyCode.A))
		{
			normalizedHorizontalSpeed = -1;
			if( transform.localScale.x > 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			if( _controller.isGrounded)
            {
                _animator.SetBool("walk", true);
            }
		}
		else
		{
			normalizedHorizontalSpeed = 0;

			if( _controller.isGrounded)
            {
                _animator.SetBool("walk", false);
            }
		}


		// we can only jump whilst grounded
		if( _controller.isGrounded && (Input.GetKeyDown( KeyCode.UpArrow ) || Input.GetKeyDown(KeyCode.Space)) )
		{
            _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            _jumpKeyHold = true;
            Invoke("EndJumpHold", jumpHoldDuration);
            _animator.SetBool("jump", true);
        }
        //Holding jump key after jump increases jump distance
        else if (!_controller.isGrounded && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space)) && _jumpKeyHold)
        {
            _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity * jumpHoldAddition);
        }


		// apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );

		// apply gravity before moving
		_velocity.y += gravity * Time.deltaTime;

		// if holding down bump up our movement amount and turn off one way platform detection for a frame.
		// this lets us jump down through one way platforms
		if( _controller.isGrounded && Input.GetKey( KeyCode.DownArrow ) )
		{
			_velocity.y *= 3f;
			_controller.ignoreOneWayPlatformsThisFrame = true;
		}

		_controller.move( _velocity * Time.deltaTime );

		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
	}

}
