using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerComponents : MonoBehaviour
{
    public PlayerInputActions playerControls;

    private InputAction move;
    private InputAction jump;
    private InputAction attack;

    private float health;

    public Animator animator;

    private Rigidbody2D playerRB;

    public BoxCollider2D hitbox;

    private bool playerFacingRight;

    //private bool canInteract = true; //this bool determines if the player should be able to move, attack, or jump (set to false when attacked)

    private bool canAttack = true; //this bool determines if the player is allowed to move or jump
    private bool canMove = true; //this bool determines if the player is allowed to attack


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

    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        jump = playerControls.Player.Jump;
        attack = playerControls.Player.Fire;

        move.Enable();
        jump.Enable();
        attack.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        attack.Disable();
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

}
