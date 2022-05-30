using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementInput : MonoBehaviour
{
    private CharacterController2D controller;

    private float horizontalMovement = 0f;
    private bool jumping = false;
    private bool sliding = false;

    public float runSpeed = 70f;

    private float jumpBufferTime = 0.3f; //adds a buffer so player jumps as soon as they touch the ground, instead of having to wait until they land to press space
    private float jumpBufferCounter;

    private PlayerComponents playerComponentScript; // using this to get reference to the canMove boolean (will determine if we should calculate user input)

    private InputAction move;
    private InputAction jump;
    private InputAction slide;


    private void Start()
    {
        controller = GetComponent<CharacterController2D>();
        playerComponentScript = GetComponent<PlayerComponents>();

        move = playerComponentScript.getMove();
        jump = playerComponentScript.getJump();

        slide = playerComponentScript.getSlide();
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
        bool canWalk = playerComponentScript.getCanWalk();

        //if player is holding slide button, then slide, otherwise stop (will be interrupted if player is no longer grounded (will probably change)
        if(slide.ReadValue<float>() > 0)
        {
            sliding = true;

        }
        else
        {
            sliding = false;
        }

        if (canWalk == true)
        {
            //using new input system
            Vector2 temp = move.ReadValue<Vector2>();
            horizontalMovement = temp.x * runSpeed;
        }
        else
            horizontalMovement = 0f;

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
        
        controller.Move(horizontalMovement * Time.fixedDeltaTime, false, jumping, sliding, jumpBufferCounter);
        jumping = false;
    }
}
