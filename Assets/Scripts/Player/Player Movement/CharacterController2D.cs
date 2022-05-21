using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 1400f;                          // Amount of force added when the player jumps.
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

	private float animVelocity = 0.0f; //velocity used for locomotion blend tree
	private int velocityHash;
	private float acceleration = 1f;

	//public int jump_count = 2; // the number of times the player can jump

	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;

    private float coyoteTime = 0.2f; // timer indicating how long the player can jump after leaving the ground (higher value means more forgiving time)
    private float coyoteTimeCounter;


	private bool isWalking;

	[SerializeField] private Animator animator;



	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	//testing github again

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

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

		//OnLandEvent.AddListener(LandAnimation);
	
    }

    private void Update()
    {

        if (m_Grounded)
        {
			//if grounded, you're not jumping
			animator.SetBool("isJumping", false);
			animator.SetBool("isGrounded", true);

            coyoteTimeCounter = coyoteTime;
			
        }
        else
        {
            //when player is mid-air, start counting the coyote time down
            coyoteTimeCounter -= Time.deltaTime;
			animator.SetBool("isGrounded", false);
		}
        

		//if we are currently falling, then we will apply the fallMultipler on the player
        if(m_Rigidbody2D.velocity.y < 0)
        {
			m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
		//if we are rising (jumping), and we let go of the jump button, then we should have a low jump multiplier applied
		else if(m_Rigidbody2D.velocity.y > 0 && !Input.GetButton("Jump"))
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
		//set locomotion velocity equal to player's speed * acceleration (this will make walking animation faster depending on movement speed)
		animVelocity = Mathf.Abs(move * acceleration);
		animator.SetFloat(velocityHash, animVelocity);
		

		if (move != 0 && m_Grounded)
        {
			isWalking = true;
			animator.SetBool("isWalking", isWalking);
        }

        else
        {
			isWalking = false;
			animator.SetBool("isWalking",isWalking);
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
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

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
		if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
		{
            // Add a vertical force to the player.
            
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
			animator.SetBool("isJumping", true);
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

}