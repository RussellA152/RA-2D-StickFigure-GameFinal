using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


//This class holds Components & Input Actions are needed by other scripts 

public class PlayerComponents : MonoBehaviour
{
    public PlayerInputActions playerControls;

    private InputAction move; //input action used for WASD movement
    private InputAction jump; //input action used for jumping 
    private InputAction lightAttack; //input action used for performing light attacks (left click)
    private InputAction heavyAttack; //input action used for performing heavy attacks (right click)
    private InputAction slide; //input action used for performing a slide (left shift)
    private InputAction backLightAttack; //input action used for performing a back light attack (turning around and performing a light attack quickly) ( A + left click or D depending on direction )

    private float health;

    public Animator animator;

    private Rigidbody2D playerRB;

    public BoxCollider2D hitbox;

    private bool playerFacingRight;

    //private bool canInteract = true; //this bool determines if the player should be able to move, attack, or jump (set to false when attacked)

    private bool canAttack = true; //this bool determines if the player is allowed to move or jump
    private bool canMove = true; //this bool determines if the player is allowed to attack
    private bool canSlide = true; //this bool determines if the player is allowed to slide


    private void Awake()
    {
        playerControls = new PlayerInputActions();
        animator = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        //Debug.Log("canMove " + canMove);
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        jump = playerControls.Player.Jump;
        lightAttack = playerControls.Player.LightAttack;
        heavyAttack = playerControls.Player.HeavyAttack;
        slide = playerControls.Player.Slide;
        backLightAttack = playerControls.Player.BackAttack;

        move.Enable();
        jump.Enable();
        lightAttack.Enable();
        heavyAttack.Enable();
        slide.Enable();
        backLightAttack.Enable();

    }
    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        lightAttack.Disable();
        heavyAttack.Disable();
        slide.Disable();
        backLightAttack.Disable();
    }


    public void setCanAttack(bool boolean)
    {
        canAttack = boolean;
    }
    //retrieves the canInteract boolean
    public bool getCanAttack()
    {
        return canAttack;
    }

    public void setCanMove(bool boolean)
    {
        canMove = boolean;
    }
    public void setCanSlide(bool boolean)
    {
        canSlide = boolean;
    }
    //retrieves the canInteract boolean
    public bool getCanMove()
    {
        return canMove;
    }

    //retrieves the health value
    public float getHP()
    {
        return health;
    }
    //retrieves the player's animator component
    public Animator getAnimator()
    {
        return animator;
    }

    //retrieves the player's rigidbody component
    public Rigidbody2D getRB()
    {
        return playerRB;
    }
    //retrives the player's hitbox 
    public BoxCollider2D getHitBox()
    {
        return hitbox;
    }

    //retrives the direction the sprite is facing
    public bool getPlayerDirection()
    {
        //update player's direction before giving 
        playerFacingRight = GetComponent<CharacterController2D>().getDirection();
        return playerFacingRight;
    }

    public InputAction getMove()
    {
        return move;
    }

    public InputAction getJump()
    {
        return jump;
    }

    public InputAction getLightAttack()
    {
        return lightAttack;
    }

    public InputAction getHeavyAttack()
    {
        return heavyAttack;
    }

    public InputAction getSlide()
    {
        return slide;
    }

    public InputAction getBackLightAttack()
    {
        return backLightAttack;
    }

}
