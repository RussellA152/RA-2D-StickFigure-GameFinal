using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementInput : MonoBehaviour
{
    private CharacterController2D controller;

    private float horizontalMovement = 0f;
    private bool jumping = false;

    public float runSpeed = 70f;

    private float jumpBufferTime = 0.3f; //adds a buffer so player jumps as soon as they touch the ground, instead of having to wait until they land to press space
    private float jumpBufferCounter;

    private PlayerComponents playerComponentScript; // using this to get reference to the canMove boolean (will determine if we should calculate user input)


    private void Start()
    {
        controller = GetComponent<CharacterController2D>();
        playerComponentScript = GetComponent<PlayerComponents>();
    }
    private void Update()
    {
        bool canMove = playerComponentScript.getCanMove();

        //if player can move, then calculate input
        //otherwise make movement = 0

        if (canMove == true)
            horizontalMovement = Input.GetAxisRaw("Horizontal") * runSpeed;
        else
            horizontalMovement = 0f;


        if (Input.GetButtonDown("Jump"))
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
