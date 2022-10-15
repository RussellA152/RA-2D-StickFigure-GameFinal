using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


// PlayerMovementInput requires the PlayerComponents script in order to access keybinds
[RequireComponent(typeof(PlayerComponents))]
public class PlayerMovementInput : MonoBehaviour
{
    [SerializeField] private CharacterController2D controller;
    [SerializeField] private PlayerComponents playerComponentScript; // using this to get reference to the canMove boolean (will determine if we should calculate user input)

    //walking value passed into CharacterController2D's Move() 
    private float horizontalMovement = 0f;
    //bools passed into CharacterController2D's Move() 
    private bool isJumping = false; 
    private bool isSliding = false;
    private bool isRolling;
    private bool isAttacking;
    private bool isGrounded;
    private bool canWalk;
    private bool canRoll;

    private float jumpBufferTime = 0.3f; //adds a buffer so player jumps as soon as they touch the ground, instead of having to wait until they land to press space
    private float jumpBufferCounter;

    //private InputAction move;
    //private InputAction jump;
    //private InputAction slide;
    //private InputAction roll;

    private bool canSlide; //determines if the player can slide (retrieved from playerComponents script)

    private void Start()
    {

        //controller = GetComponent<CharacterController2D>();
        //playerComponentScript = GetComponent<PlayerComponents>();


        //move = playerComponentScript.GetMove();
        //jump = playerComponentScript.GetJump();

        //slide = playerComponentScript.GetSlide();
        //roll = playerComponentScript.GetRoll();

        ////subscribe the slide and roll to their respective functions
        //slide.performed += SlideInput;
        //slide.canceled += SlideInput;
    }


    private void OnDisable()
    {
        //AttackController.instance.onAttack -= SetRollingFalse;
    }

    private void Update()
    {
        //update canWalk and canRoll to see if the player is allowed to roll or walk
        canWalk = playerComponentScript.GetCanWalk();
        canRoll = playerComponentScript.GetCanRoll();

        //update isAttacking to see if player is currently attacking
        isAttacking = AttackController.instance.GetPlayerIsAttacking();

        //update isGrounded to see if player is currently grounded
        isGrounded = playerComponentScript.GetPlayerIsGrounded();

        canSlide = playerComponentScript.GetCanSlide();

        jumpBufferCounter -= Time.deltaTime;

        //CheckRoll(isAttacking, canRoll, isGrounded, isSliding);

        //CheckWalk(canWalk);

        //CheckJump();


    }
    private void FixedUpdate()
    {
        //Pass input values into the Move() function from CharacterController2D
        //the crouch (second parameter) is false because the game will probably not feature a crouch button (unless I implement a crouch sweep or something)
        controller.Move(horizontalMovement * Time.fixedDeltaTime, false, isJumping, isSliding,isRolling, jumpBufferCounter);

        //after calling Move, set jumping back to false (will be true when player input for jumping is detected)
        isJumping = false;

    }

    public void PlayerMove(float input)
    {
        if (canWalk)
        {
            horizontalMovement = input;
        }
        else
        {
            horizontalMovement = 0f;
        }
    }
    public void Jump()
    {
        isJumping = true;
        jumpBufferCounter = jumpBufferTime;
    }

    public void Roll(bool hasRolled)
    {
        if (canRoll && hasRolled && isGrounded && !isAttacking && !isSliding)
        {
            SetIsRolling(true);
        }
    }
    public void Slide(InputAction.CallbackContext context)
    {
        //if player is holding slide button, then slide, otherwise stop (will be interrupted if player is no longer grounded)
        if (context.performed)
        {
            isSliding = true;

            //although this is a movement ability, it deals damage to enemies that it comes into contact with, so we must set PlayerIsAttacking to true here
            AttackController.instance.SetPlayerIsAttacking(true);

        }
        else if (context.canceled)
        {
            isSliding = false;
        }
    }

    ////check for walking input from player
    //private void CheckWalk(bool canWalk)
    //{
    //    if (canWalk == true)
    //    {
    //        //using new input system
    //        Vector2 movementInput = move.ReadValue<Vector2>();
    //        horizontalMovement = movementInput.x;
    //    }
    //    else
    //        horizontalMovement = 0f;
    //}

    ////check for roll input from player
    //private void CheckRoll(bool playerIsAttacking,bool canRoll,bool isGrounded, bool isSliding)
    //{
    //    //if player is allowed to roll (they must be grounded, not attacking, and not sliding)
    //    if (canRoll && roll.triggered && isGrounded && !playerIsAttacking && !isSliding)
    //    {
    //        SetIsRolling(true);
    //    }
    //}

    ////check for jump input from player
    //private void CheckJump()
    //{
    //    //if player pressed jump button..
    //    //also start jump buffer counter
    //    if (jump.triggered)
    //    {
    //        isJumping = true;
    //        jumpBufferCounter = jumpBufferTime;
    //    }
    //    else
    //    {
    //        jumpBufferCounter -= Time.deltaTime;
    //    }
    //}
    ////check for slide input from player 
    //private void SlideInput(InputAction.CallbackContext context)
    //{
    //    //if player is holding slide button, then slide, otherwise stop (will be interrupted if player is no longer grounded)
    //    if (context.performed)
    //    {
    //        isSliding = true;

    //        //although this is a movement ability, it deals damage to enemies that it comes into contact with, so we must set PlayerIsAttacking to true here
    //        AttackController.instance.SetPlayerIsAttacking(true);

    //    }
    //    else if (context.canceled)
    //    {
    //        isSliding = false;
    //    }
    //}

    public void SetIsRolling(bool boolean)
    {
        isRolling = boolean;
    }





}



