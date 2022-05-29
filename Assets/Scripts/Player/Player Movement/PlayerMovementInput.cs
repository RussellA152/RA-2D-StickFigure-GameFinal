using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementInput : MonoBehaviour
{
    private CharacterController2D controller;

    private float horizontalMovement = 0f;
    private bool jumping = false;

    public float runSpeed = 70f;

    private float jumpBufferTime = 0.3f; //adds a buffer so player jumps as soon as they touch the ground, instead of having to wait until they land to press space
    private float jumpBufferCounter;

    private PlayerComponents playerComponentScript; // using this to get reference to the canMove boolean (will determine if we should calculate user input)

    private InputAction move;
    private InputAction jump;


    private void Start()
    {
        controller = GetComponent<CharacterController2D>();
        playerComponentScript = GetComponent<PlayerComponents>();

        move = playerComponentScript.getMove();
        jump = playerComponentScript.getJump();
    }


    private void OnEnable()
    {
        //move = playerComponentScript.getMove();
        //jump = playerComponentScript.getJump();

        //move.Enable();
        //jump.Enable();
    }
    private void OnDisable()
    {
        //move.Disable();
        //jump.Disable();
    }


    private void Update()
    {
        bool canMove = playerComponentScript.getCanMove();
        //Debug.Log(horizontalMovement);
        //if player can move, then calculate input
        //otherwise make movement = 0

        if (canMove == true)
        {
            Vector2 temp = move.ReadValue<Vector2>();
            //using new input system
            horizontalMovement = temp.x * runSpeed;
            //horizontalMovement = Input.GetAxisRaw("Horizontal") * runSpeed;
        }
        else
            horizontalMovement = 0f;

        //Debug.Log("Speed: " + horizontalMovement);

        if (jump.triggered)
        {
            jumping = true;
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        //Move our character here
        //the crouch (second parameter) is false because the game will probably not feature a crouch button (unless I implement a crouch sweep or something)
        
        controller.Move(horizontalMovement * Time.fixedDeltaTime, false, jumping, jumpBufferCounter);
        jumping = false;
    }
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementInput : MonoBehaviour
{
    public PlayerInputActions playerControls;

    private CharacterController2D controller;

    private float horizontalMovement = 0f;
    private bool jumping = false;

    public float runSpeed = 70f;

    private float jumpBufferTime = 0.3f; //adds a buffer so player jumps as soon as they touch the ground, instead of having to wait until they land to press space
    private float jumpBufferCounter;

    private PlayerComponents playerComponentScript; // using this to get reference to the canMove boolean (will determine if we should calculate user input)

    private InputAction move;
    private InputAction jump;



    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void Start()
    {
        controller = GetComponent<CharacterController2D>();
        playerComponentScript = GetComponent<PlayerComponents>();
    }


    private void OnEnable()
    {
        move = playerControls.Player.Move;
        jump = playerControls.Player.Jump;

        move.Enable();
        jump.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }


    private void Update()
    {
        bool canMove = playerComponentScript.getCanMove();

        //if player can move, then calculate input
        //otherwise make movement = 0

        if (canMove == true)
            //using new input system
            horizontalMovement = move.ReadValue<float>();
        //horizontalMovement = Input.GetAxisRaw("Horizontal") * runSpeed;
        else
            horizontalMovement = 0f;


        if (jump.ReadValue<bool>())
        {
            jumping = true;
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        //Move our character here
        //the crouch (second parameter) is false because the game will probably not feature a crouch button (unless I implement a crouch sweep or something)

        controller.Move(horizontalMovement * Time.fixedDeltaTime, false, jumping, jumpBufferCounter);
        jumping = false;
    }
}


*/
