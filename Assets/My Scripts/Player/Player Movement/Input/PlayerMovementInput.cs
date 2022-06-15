using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


// PlayerMovementInput requires the PlayerComponents script in order to access keybinds
[RequireComponent(typeof(PlayerComponents))]
public class PlayerMovementInput : MonoBehaviour
{
    private CharacterController2D controller;

    //walking value passed into CharacterController2D's Move() 
    private float horizontalMovement = 0f;
    //bools passed into CharacterController2D's Move() 
    private bool jumping = false; 
    private bool sliding = false;
    private bool rolling;


    [Header("Walking Speed")]
    [SerializeField] private float runSpeed = 70f;

    private float jumpBufferTime = 0.3f; //adds a buffer so player jumps as soon as they touch the ground, instead of having to wait until they land to press space
    private float jumpBufferCounter;

    private PlayerComponents playerComponentScript; // using this to get reference to the canMove boolean (will determine if we should calculate user input)

    private InputAction move;
    private InputAction jump;
    private InputAction slide;
    private InputAction roll;

    private bool canSlide; //determines if the player can slide (retrieved from playerComponents script)

    private void Start()
    {



        //Application.targetFrameRate = 20;
        controller = GetComponent<CharacterController2D>();
        playerComponentScript = GetComponent<PlayerComponents>();


        move = playerComponentScript.GetMove();
        jump = playerComponentScript.GetJump();

        slide = playerComponentScript.GetSlide();
        roll = playerComponentScript.GetRoll();

        //subscribe the slide and roll to their respective functions
        slide.performed += SlideInput;
        slide.canceled += SlideInput;

        //roll.performed += RollInput;
        //roll.canceled += RollInput;
    
    }


    private void OnDisable()
    {
        //AttackController.instance.onAttack -= SetRollingFalse;
    }

    private void Update()
    {
        //update canWalk and canRoll to see if the player is allowed to roll or walk
        bool canWalk = playerComponentScript.GetCanWalk();
        bool canRoll = playerComponentScript.GetCanRoll();

        //update isAttacking to see if player is currently attacking
        bool isAttacking = AttackController.instance.GetPlayerIsAttacking();

        //update isGrounded to see if player is currently grounded
        bool isGrounded = playerComponentScript.GetPlayerIsGrounded();

        canSlide = playerComponentScript.GetCanSlide();

        CheckRoll(isAttacking, canRoll, isGrounded, sliding);

        CheckWalk(canWalk);

        CheckJump();
            

    }
    private void FixedUpdate()
    {
        //Move our character here
        //the crouch (second parameter) is false because the game will probably not feature a crouch button (unless I implement a crouch sweep or something)
        controller.Move(horizontalMovement * Time.fixedDeltaTime, false, jumping, sliding,rolling, jumpBufferCounter);

        //after calling Move, set jumping and rolling back to false (they will be true when player input for jumping and rolling is detected)
        jumping = false;

        //rolling = false;
    }

    //check for walking input from player
    private void CheckWalk(bool canWalk)
    {
        if (canWalk == true)
        {
            //using new input system
            Vector2 movementInput = move.ReadValue<Vector2>();
            horizontalMovement = movementInput.x * runSpeed;
        }
        else
            horizontalMovement = 0f;
    }

    //check for roll input from player
    private void CheckRoll(bool playerIsAttacking,bool canRoll,bool isGrounded, bool isSliding)
    {
        //if player is allowed to roll (they must be grounded, not attacking, and not sliding)
        if (canRoll && roll.triggered && !slide.triggered && isGrounded && !playerIsAttacking && !sliding)
        {
            SetRolling(true);
        }
    }

    //check for jump input from player
    private void CheckJump()
    {
        //if player pressed jump button..
        //also start jump buffer counter
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
    private void SlideInput(InputAction.CallbackContext context)
    {
        //if player is holding slide button, then slide, otherwise stop (will be interrupted if player is no longer grounded)
        if (context.performed)
        {
            Debug.Log("SLIDE IS TRUE!");
            sliding = true;
        }
        else if (context.canceled)
        {
            Debug.Log("SLIDE IS FALSE!");
            sliding = false;
        }
        else
        {
            sliding = false;
        }
    }

    public void SetRolling(bool boolean)
    {
        rolling = boolean;
    }





}



