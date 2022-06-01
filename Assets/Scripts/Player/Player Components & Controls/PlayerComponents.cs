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
    private InputAction backLightAttackLeft; //input action used for performing a back light attack (turning around and performing a light attack quickly) ( A for turning left )
    private InputAction backLightAttackRight; //input action used for performing a back light attack (turning around and performing a light attack quickly) ( D for turning right )

    private float health;

    public Animator animator;

    private Rigidbody2D playerRB;

    public BoxCollider2D hitbox;

    private bool playerFacingRight;

    //private bool canInteract = true; //this bool determines if the player should be able to move, attack, or jump (set to false when attacked)

    private bool canAttack = true; //this bool determines if the player is allowed to attack

    //(Set to false by default since player hasn't turned around at the start of the game
    private bool canBackAttack = false; //this bool determines if the player to perform a back attack (turning around and attacking) 
    private bool canMove = true; //this bool determines if the player is allowed to walk and jump (both)
    private bool canSlide = true; //this bool determines if the player is allowed to slide

    private bool canJump = true; //this bool determines if the player is allowed to walk (only)
    private bool canWalk = true; //this bool determines if the player is allowed to jump (only)


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
        backLightAttackLeft = playerControls.Player.BackAttackLeft;
        backLightAttackRight = playerControls.Player.BackAttackRight;

        move.Enable();
        jump.Enable();
        lightAttack.Enable();
        heavyAttack.Enable();
        slide.Enable();
        backLightAttackLeft.Enable();
        backLightAttackRight.Enable();

    }
    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        lightAttack.Disable();
        heavyAttack.Disable();
        slide.Disable();
        backLightAttackLeft.Disable();
        backLightAttackRight.Disable();
    }


    public void SetCanAttack(bool boolean)
    {
        canAttack = boolean;
        canBackAttack = boolean;

    }
    public void SetCanBackAttack(bool boolean)
    {
        canBackAttack = boolean;
    }

    //retrieves the canAttack boolean
    public bool GetCanAttack()
    {
        return canAttack;
    }
    public bool GetCanBackAttack()
    {
        return canBackAttack;
    }

    public void SetCanMove(bool boolean)
    {
        canJump = boolean;
        canWalk = boolean;
    }
    public void SetCanJump(bool boolean)
    {
        canJump = boolean;
    }
    public void SetCanWalk(bool boolean)
    {
        canJump = boolean;
    }

    public void SetCanSlide(bool boolean)
    {
        canSlide = boolean;
    }
    public bool GetCanJump()
    {
        return canJump;
    }
    public bool GetCanWalk()
    {
        return canWalk;
    }
    //retrieves the canMove boolean
    public bool GetCanMove()
    {
        return canMove;
    }

    //retrieves the health value
    public float GetHP()
    {
        return health;
    }
    //retrieves the player's animator component
    public Animator GetAnimator()
    {
        return animator;
    }

    //retrieves the player's rigidbody component
    public Rigidbody2D GetRB()
    {
        return playerRB;
    }
    //retrives the player's hitbox 
    public BoxCollider2D GetHitBox()
    {
        return hitbox;
    }

    //retrives the direction the sprite is facing
    public bool GetPlayerDirection()
    {
        //update player's direction before giving 
        playerFacingRight = GetComponent<CharacterController2D>().GetDirection();
        return playerFacingRight;
    }

    public InputAction GetMove()
    {
        return move;
    }

    public InputAction GetJump()
    {
        return jump;
    }

    public InputAction GetLightAttack()
    {
        return lightAttack;
    }

    public InputAction GetHeavyAttack()
    {
        return heavyAttack;
    }

    public InputAction GetSlide()
    {
        return slide;
    }

    public InputAction GetBackLightAttackLeft()
    {
        return backLightAttackLeft;
    }
    public InputAction GetBackLightAttackRight()
    {
        return backLightAttackRight;
    }
    //might put this in a different script later (could there be a memory leak here?)
    // ONLY WORKS WITH A KEYBOARD AT THE MOMENT
    public void RebindBackAttack(string originalBind, string newBind)
    {
        //Debug.Log("REBIND START!");
        //if(originalBind.Equals("a"))
            //backLightAttack.ChangeBindingWithPath("<Keyboard>/a").WithPath("<Keyboard>/d");
        //else if (originalBind.Equals("d"))
            //backLightAttack.ChangeBindingWithPath("<Keyboard>/d").WithPath("<Keyboard>/a");

        //Debug.Log("REBIND END!");
    }

}
