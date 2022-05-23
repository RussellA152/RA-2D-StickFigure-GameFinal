using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponents : MonoBehaviour
{
    private float health;
    public Animator animator;
    private Rigidbody2D playerRB;
    private CharacterController2D characterController2D;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        characterController2D = GetComponent<CharacterController2D>();
        playerRB = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        
    }

    public float getHP()
    {
        return health;
    }
    public Animator getAnimator()
    {
        return animator;
    }

    public Rigidbody2D getRB()
    {
        return playerRB;
    }


    public CharacterController2D getControllerScript()
    {
        return characterController2D;
    }
}
