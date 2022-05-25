using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponents : MonoBehaviour
{
    private float health;

    public Animator animator;

    private Rigidbody2D playerRB;

    public GameObject hitbox;

    private bool canInteract = true; //this bool determines if the player should be able to move, attack, or jump (set to false when attacked)

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerRB = GetComponent<Rigidbody2D>();

        hitbox = GameObject.Find("Player Hit Box");
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            setCanInteract(false);
        }
        else
        {
            setCanInteract(true);
        }
    }

    //sets canInteract equal to the given boolean value
    public void setCanInteract(bool boolean)
    {
        canInteract = boolean;
    }
    //retrieves the canInteract boolean
    public bool getCanInteract()
    {
        return canInteract;
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

    public GameObject getHitBox()
    {
        return hitbox;
    }

}
