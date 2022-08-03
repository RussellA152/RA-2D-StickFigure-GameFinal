using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.InputSystem;


// CharacterController2D requires the PlayerComponents script in order to access keybinds
[RequireComponent(typeof(PlayerComponents))]
public class CharacterController2D : MonoBehaviour
{
	public PlayerInputActions playerControls;

	[SerializeField] private PlayerComponents playerComponentScript;

	[Header("Speed Properties")]
	[SerializeField] private float runSpeed = 70f;                              //general movement speed of the player
	[SerializeField] private float m_JumpForce = 1400f;                         // Amount of force added when the player jumps.
	private float minSpeedMultiplierRequirement;                                //this is the minimum value that the player must be moving at in order to increase movement speed (for the speed multiplier to begin)
																				//we need this value for gamepads, otherwise the player can build up their speed multiplier while moving slow (they should atleast be moving relatively fast)
	[SerializeField] private float speedMultiplier = 10f;                       // this float is applied to the regular movement speed (allows movement to gradually increase)
	[SerializeField] private float maxSpeedMultiplier = 20f;					//this is the maximum value that the speed multiplier can reach, speed multiplier won't go further than this (increasing this value means player will increase speed more)
	
	

	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement

	[Header("Cooldowns")]
	[SerializeField] private float rollCooldownTimer;
	//[SerializeField] private float slideCooldownTimer;
	private bool rollCooldownCoroutineOccurred = false; //this bool is used for making sure the roll cooldown coroutine only occurs once
	//private bool slideCooldownCoroutineOccurred = false; //this bool is used for making sure the slide cooldown coroutine only occurs once

	[Header("Allow AirControl?")]
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;

	[Header("Ground Checks")]
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

	//public int jump_count = 2; // the number of times the player can jump

	[Header("Falling Speed Properties")]
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;

    private float coyoteTime = 0.2f; // timer indicating how long the player can jump after leaving the ground (higher value means more forgiving time)
    private float coyoteTimeCounter;


	private bool isWalking; //bool if player is walking
	private bool canMove; //determines if player can walk and jump (retrieved from playerComponents script)
	private bool canWalk; //determines if player can walk (retrieved from playerComponents script)
	private bool canJump; //determines if player can jump (retrieved from playerComponents script)
	private bool canFlip; //determines if the player's sprite can flip (retrieved from playerComponents script)
	private bool canRoll; //determines if the player can roll (retrieved from playerComponents script)
	private bool canSlide; //determines if the player can slide (retrieved from playerComponents script)

	[HideInInspector]
	public Animator animator;

	private float animVelocity = 0.0f; //velocity used for locomotion blend tree

	//using hashvalues for setBool & setFloat is faster than using strings like "isWalking"
	private int velocityHash; //the hash value of our animator's velocity parameter
	private int isWalkingHash; //hash value of our animator's isWalking parameter
	private int isJumpingHash; //hash value of our animator's isJumping parameter
	private int isGroundedHash; //hash value of our animator's isGrounded parameter
	private int isSlidingHash; //hash value of our animator's isSliding parameter
	private int isRollingHash; //hash value of our animator's isRolling parameter


	//private InputAction move;
	private InputAction jump;
	private InputAction turnRight;
	private InputAction turnLeft;


	private float backAttackTimer = 0.35f; //time allowed for player to perform a back attack (once this hits 0, the player must turn around again to perform a back attack)

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

		//increases performance to use hashvalues instead of reading strings
		velocityHash = Animator.StringToHash("Velocity");
		isGroundedHash = Animator.StringToHash("isGrounded");
		isWalkingHash = Animator.StringToHash("isWalking");
		isJumpingHash = Animator.StringToHash("isJumping");
		isSlidingHash = Animator.StringToHash("isSliding");
		isRollingHash = Animator.StringToHash("isRolling");

		//playerComponentScript = GetComponent<PlayerComponents>();

		jump = playerComponentScript.GetJump();
		turnLeft = playerComponentScript.GetTurnLeft();
		turnRight = playerComponentScript.GetTurnRight();
		m_Rigidbody2D = playerComponentScript.GetRB();
		animator = playerComponentScript.GetAnimator();


		UpdateMinimumSpeedMultiplierRequirement();
	
    }

    private void Update()
    {

		//Debug.Log("Speed multiplier = " + speedMultiplier);

		//decrement back attack timer until it hits 0 (player can no longer back attack
		if (backAttackTimer > 0f)
			backAttackTimer -= Time.deltaTime;
		else
			backAttackTimer = 0f;

		//check if back attack should be true or false depending on timer
		CheckBackAttack();

		//always updating the canMove bool to check if player is allowed to move and jump
		UpdatePlayerComponents();


		//if grounded, animator's isJumping is set to false, and isGrounded parameter is set to true
		if (m_Grounded)
        {
			animator.SetBool(isJumpingHash, false);
			animator.SetBool(isGroundedHash, true);

			coyoteTimeCounter = coyoteTime;
			
        }
		//when player is mid-air, start counting the coyote time down
		else
		{
            coyoteTimeCounter -= Time.deltaTime;
			animator.SetBool(isGroundedHash, false);
		}

		//if we are currently falling, then we will apply the fallMultipler on the player
		ApplyFallMultiplier();
	}

    private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// set playercomponent's isGrounded to false so other scripts can see 
		playerComponentScript.SetIsGrounded(false);

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;

				// set playercomponent's isGrounded to true so other scripts can see 
				playerComponentScript.SetIsGrounded(true);

				//Player has just landed
				if (!wasGrounded)
					OnLandEvent.Invoke();
					
			}
		}
	}
	public void Move(float move, bool crouch, bool jump, bool wantsToSlide, bool wantsToRoll,float jumpBufferCounter)
	{

		//multiply the movement by the running speed set in the inspector
		move *= runSpeed;

		//set isSliding bool parameter inside of player animator to true or false depending on player input
		if (m_Grounded && canSlide && wantsToSlide)
        {
			animator.SetBool(isSlidingHash, wantsToSlide);

		}
        else
        {
			animator.SetBool(isSlidingHash, false);
		}

		//check if player is allowed to roll... (need to be grounded otherwise cooldown will start mid-air)
        if (canRoll && wantsToRoll  && m_Grounded)
        {
			//set isRolling bool parameter inside of player animator to true or false depending on player input
			animator.SetBool(isRollingHash, wantsToRoll);

			//if the roll cooldown coroutine is already running, don't run it again (only one at a time)
			if(!rollCooldownCoroutineOccurred)
			StartCoroutine(RollCooldown());

		}
        else
        {
			animator.SetBool(isRollingHash, false);
		}
		

		//if player is moving and grounded, play walking animation
		if (move != 0 && m_Grounded)
		{
			isWalking = true;
			animator.SetBool(isWalkingHash, isWalking);
		}

		else
		{
			isWalking = false;
			animator.SetBool(isWalkingHash, isWalking);
		}

		//Once the player has reached the minimum walking speed requirement (about 1.1f), then allow them to increase their movement speed further
		if (move >= minSpeedMultiplierRequirement || move <= -minSpeedMultiplierRequirement)
        {
			//limit the movement speedMultipler to 20 so the player can move too fast
			if(speedMultiplier < maxSpeedMultiplier)
				speedMultiplier += 0.1f;
		}	
		//when player stops moving, reset speedMultiplier (also reset when the sprite is flipped)
		else
			speedMultiplier = 10f;



		//if you cannot interact or walk, then set move to 0 (player will not be able to move)
		if (!canMove || !canWalk)
			move = 0f;
	

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


			//set locomotion velocity equal to player's current speed (this will make walking animation faster depending on movement speed)
			//is divided by 12 so that the animation doesn't move too fast 
			animVelocity = Mathf.Abs(move * speedMultiplier / 12f);

			//setting the animator's velocity equal to animVelocity
			animator.SetFloat(velocityHash, animVelocity);

			// If the input is moving the player right and the player is facing left...
			if (turnRight.ReadValue<float>() > 0 && turnLeft.ReadValue<float>() == 0 && !m_FacingRight && canFlip)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (turnLeft.ReadValue<float>() > 0  && turnRight.ReadValue<float>() == 0 && m_FacingRight && canFlip)
			{
				// ... flip the player.
				Flip();
			}
            
		}
		// If the player should jump...
        // BEFORE: I was checking if player was allowed to jump and they were grounded, but i substituted
        // the grounded condition with checking the coyoteTimeCounter instead
		if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && canJump)
		{

			//reset y velocity when jumping so player can get a high jump height with coyote time
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x,0f);

			//player is no longer grounded when jumping
			m_Grounded = false;

			// set playercomponent's isGrounded to false so other scripts can see 
			playerComponentScript.SetIsGrounded(false);


			// Add a vertical force to the player.
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			
			//set CoyoteTime and jumpBuffer to 0
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;

			//allow isJumping to be true
			animator.SetBool(isJumpingHash, true);

		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		//reset back attack timer
		backAttackTimer = 0.35f;

		//player has turned around, they are now allowed to perform a back attack
		playerComponentScript.SetCanBackAttack(true);
		//reset speed multiplier (fixes bug where player can keep momentum when turning around
		speedMultiplier = 10f;


		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private void ApplyFallMultiplier()
    {
		//if we are currently falling, then we will apply the fallMultipler on the player
		if (m_Rigidbody2D.velocity.y < 0)
		{
			m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		}
		//if we are rising (jumping), and we let go of the jump button, then we should have a low jump multiplier applied
		else if (m_Rigidbody2D.velocity.y > 0 && jump.ReadValue<float>() == 0f)
		{
			m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}
	}

	//return the direction of the player
	public bool GetDirection()
    {
		return m_FacingRight;
    }


	private void CheckBackAttack()
    {
		//if back attack timer reaches 0, then the player must turn around again
		if (backAttackTimer <= 0f)
			playerComponentScript.SetCanBackAttack(false);
		//Debug.Log(backAttackTimer);
	}

	//invoked at start and should be invoked whenever the running speed is changed (say we pick up an item that increases our movement speed)
	public void UpdateMinimumSpeedMultiplierRequirement()
    {
		minSpeedMultiplierRequirement = runSpeed * 1 / 60; //since movement speed could be changed throughout runtime.. we need to find calculate the minimum speed multiplier requirement 
														   //we multiply the runSpeed by a small value like 1/60 so that even small running speeds like 10 will be able to activate the speed multiplier
														   
	}

	private void UpdatePlayerComponents()
    {
		//updates all conditions to perform certain movements (taken from the PlayerComponent.cs script)
		canMove = playerComponentScript.GetCanMove();
		canWalk = playerComponentScript.GetCanWalk();
		canJump = playerComponentScript.GetCanJump();
		canFlip = playerComponentScript.GetCanFlip();
		canRoll = playerComponentScript.GetCanRoll();
		canSlide = playerComponentScript.GetCanSlide();

	}

	IEnumerator RollCooldown()
    {
		//Debug.Log("Roll cooldown started");

		//cooldown coroutine has occurred
		rollCooldownCoroutineOccurred = true;

		//after rolling, player must wait a certain time until they can roll again
		playerComponentScript.SetCanRoll(false);

		//wait a certain amount of time, then allow the player to roll again (roll input is still detected from PlayerMovementInput.cs , but nothing will happen if canRoll is false)
		yield return new WaitForSeconds(rollCooldownTimer);

		//cooldown coroutine has finished
		rollCooldownCoroutineOccurred = false;

		//allow player to roll again
		playerComponentScript.SetCanRoll(true);

		//Debug.Log("Roll cooldown ended");
	}
}
