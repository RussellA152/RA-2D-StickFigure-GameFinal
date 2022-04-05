using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public CharacterController2D controller;

    private float horizontalMovement = 0f;
    private bool jumping = false;

    public float runSpeed = 40f;
    private void Start()
    {
        
    }
    private void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jumping = true;
        }
    }
    private void FixedUpdate()
    {
        //Move our character here
        //the crouch (second parameter) is false because the game will probably not feature a crouch button (unless I implement a crouch sweep or something)
        controller.Move(horizontalMovement * Time.fixedDeltaTime, false, jumping);
        jumping = false;
    }
}
