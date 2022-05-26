using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponents : MonoBehaviour
{
    private float health;

    public Animator animator;

    private Rigidbody2D playerRB;

    public BoxCollider2D hitbox;

    //private bool canInteract = true; //this bool determines if the player should be able to move, attack, or jump (set to false when attacked)

    private bool canAttack = true; //this bool determines if the player is allowed to move or jump
    private bool canMove = true; //this bool determines if the player is allowed to attack


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerRB = GetComponent<Rigidbody2D>();

    }
    private void Start()
    {
        
    }

    private void Update()
    {
        //Debug.Log("canInteract is: " + canInteract);

        //if (Input.GetKey(KeyCode.E))
        //{
        ////    setCanInteract(false);
        //}
        //else
        //{
        //    setCanInteract(true);
        //}
    }

    //sets canInteract equal to the given boolean value
    //public void setCanInteract(bool boolean)
    //{
      //  canInteract = boolean;
    //}
    //retrieves the canInteract boolean
    //public bool getCanInteract()
    //{
      //  return canInteract;
    //}

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

}
