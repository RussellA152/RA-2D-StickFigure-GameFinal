using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.InputSystem;

public class CharacterController2D : MonoBehaviour
{
	public PlayerInputActions playerControls;


	private PlayerComponents playerComponentScript;

	[SerializeField] private float m_JumpForce = 1400f;                          // Amount of force added when the player jumps.
	[SerializeField] private float speedMultiplier = 10f;                       // this float is applied to the regular movement speed (allows movement to gradually increase)
	[SerializeField] private float maxSpeedMultiplier = 20f;
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.


	private Vector3 m_Velocity = Vector3.zero;

	private float acceleration = 1f; //acceleration of player's movement

	//public int jump_count = 2; // the number of times the player can jump

	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;

    private float coyoteTime = 0.2f; // timer indicating how long the player can jump after leaving the ground (higher value means more forgiving time)
    private float coyoteTimeCounter;


	private bool isWalking; //bool if player is walking
	private bool canMove; //determines if player can walk and jump (retrieved from playerComponents script

	public Animator animator;

	private float animVelocity = 0.0f; //velocity used for locomotion blend tree

	//using hashvalues for setBool & setFloat is faster than using strings like "isWalking"
	private int velocityHash; //the hash value of our animator's velocity parameter
	private int isWalkingHash; //hash value of our animator's isWalking parameter
	private int isJumpingHash; //hash value of our animator's isJumping parameter
	private int isGroundedHash; //hash value of our animator's isGrounded parameter


	private InputAction move;
	private InputAction jump;



	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Awake()
	{


		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

    private void Start()
    {


		isWalking = false;

		//increases performance
		velocityHash = Animator.StringToHash("Velocity");
		isGroundedHash = Animator.StringToHash("isGrounded");
		isWalkingHash = Animator.StringToHash("isWalking");
		isJumpingHash = Animator.StringToHash("isJumping");

		playerComponentScript = GetComponent<PlayerComponents>();

		move = playerComponentScript.getMove();
		jump = playerComponentScript.getJump();

		m_Rigidbody2D = playerComponentScript.getRB();
		animator = playerComponentScript.getAnimator();


		
	
    }

    private void Update()
    {

        //if (Input.GetKey(KeyCode.S))
        //{
			//animator.SetBool("isSliding", true);
        //}
        //else
        //{
			//animator.SetBool("isSliding", false);
        //}


		//always updating the canInteract bool to check if player is allowed to move and jump
		canMove = playerComponentScript.getCanMove();
		//Debug.Log("Velocity is : " + animVelocity);
		if (m_Grounded)
        {
			//if grounded, you're not jumping
			animator.SetBool(isJumpingHash, false);
			animator.SetBool(isGroundedHash, true);

			coyoteTimeCounter = coyoteTime;
			
        }
        else
        {
            //when player is mid-air, start counting the coyote time down
            coyoteTimeCounter -= Time.deltaTime;
			animator.SetBool(isGroundedHash, false);
		}
        

		//if we are currently falling, then we will apply the fallMultipler on the player
        if(m_Rigidbody2D.velocity.y < 0)
        {
			m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
		//if we are rising (jumping), and we let go of the jump button, then we should have a low jump multiplier applied
		else if(m_Rigidbody2D.velocity.y > 0 && jump.ReadValue<float>() == 0f)
        {
			m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}
    }

    private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;

				//Player has just landed
				if (!wasGrounded)
					OnLandEvent.Invoke();
					
			}
		}
	}


	public void Move(float move, bool crouch, bool jump, float jumpBufferCounter)
	{
		//if you're moving then gradually increase movement speed
		if (move > 0 || move < 0)
        {
			//limit the movement speedMultipler to 20 so the player can move too fast
			if(speedMultiplier < maxSpeedMultiplier)
				speedMultiplier += 0.1f;
		}
			
		else
			speedMultiplier = 10f;



		//if you cannot interact, then set move to 0 (player will not be able to move)
		if (!canMove)
			move = 0f;
		

		if (move != 0 && m_Grounded)
        {
			isWalking = true;
			animator.SetBool(isWalkingHash, isWalking);
        }

        else
        {
			isWalking = false;
			animator.SetBool(isWalkingHash,isWalking);
		}

		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			}
			else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * speedMultiplier, m_Rigidbody2D.velocity.y);

			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);


			//set locomotion velocity equal to player's speed * acceleration (this will make walking animation faster depending on movement speed)
			animVelocity = Mathf.Abs((targetVelocity.x * acceleration)/12);
			//setting the animator's velocity equal to animVelocity
			animator.SetFloat(velocityHash, animVelocity);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
            
		}
		// If the player should jump...
        // BEFORE: I was checking if player was allowed to jump and they were grounded, but i substituted
        // the grounded condition with checking the coyoteTimeCounter instead
		if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && canMove)
		{

			//reset y velocity when jumping so player can get a high jump height with coyote time
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x,0f);

			m_Grounded = false;

			// Add a vertical force to the player.
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			//Debug.Log("Y Velocity: " + m_Rigidbody2D.velocity.y);
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
			animator.SetBool(isJumpingHash, true);

		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public bool getDirection()
    {
		return m_FacingRight;
    }

 

}
