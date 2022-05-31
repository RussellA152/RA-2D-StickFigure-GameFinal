using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour
{
    private PlayerComponents playerComponentScript;

    public static AttackController instance;

    public Animator animator;

    private bool canAttack; //determines if player is allowed to attack
    private bool canBackAttack; //determines if player is allowed to do a back attack

    public bool isAttacking = false;
    public bool isHeavyAttacking = false;
    public bool isBackAttacking = false;

    private InputAction lightAttack;
    private InputAction heavyAttack;

    private InputAction backLightAttackLeft;
    private InputAction backLightAttackRight;



    private void Awake()
    {
        instance = this;
    
    }

    private void OnEnable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        playerComponentScript = GetComponent<PlayerComponents>();
        animator = playerComponentScript.GetAnimator();

        lightAttack = playerComponentScript.GetLightAttack();
        heavyAttack = playerComponentScript.GetHeavyAttack();
        backLightAttackLeft = playerComponentScript.GetBackLightAttackLeft();
        backLightAttackRight = playerComponentScript.GetBackLightAttackRight();

        //backLightAttackLeft.performed += BackLightAttackLeft;
        //backLightAttackRight.performed += BackLightAttackRight;
        //lightAttack.performed += LightAttack;
        //heavyAttack.performed += HeavyAttack;
        //backLightAttack.performed += BackLightAttack;
    }

    // Update is called once per frame
    void Update()
    {
        
        canAttack = playerComponentScript.GetCanAttack();
        canBackAttack = playerComponentScript.GetCanBackAttack();

        Attack();



    }
    private void Attack()
    {
        //instead of using event systems with context, I am using .triggered and if statements to create a priority system
        // .triggered means the button was pressed

        if (backLightAttackLeft.triggered && canBackAttack && !isBackAttacking)
        {
            isBackAttacking = true;
            Debug.Log("Back Attack TO THE LEFT!");
            //Debug.Log("BACK ATTACK!");
        }
        else if(backLightAttackRight.triggered && canBackAttack && !isBackAttacking)
        {
            isBackAttacking = true;
            Debug.Log("Back Attack TO THE RIGHT!");
        }

        else if (lightAttack.triggered && canAttack && !isAttacking)
        {
            isAttacking = true;
            //Debug.Log("LIGHT ATTACK!");
        }

        else if (heavyAttack.triggered && canAttack && !isHeavyAttacking)
        {
            isHeavyAttacking = true;
            //Debug.Log("HEAVY ATTACK!");
        }
    }

    
    //public void LightAttack(InputAction.CallbackContext context)
    //{
        //if (context.performed && canAttack &&!isAttacking && !isBackAttacking)
        //{
            //isAttacking = true;
            //Debug.Log("Light Attack!");
        //}
    //}

    //public void HeavyAttack(InputAction.CallbackContext context)
    //{
        //if (context.performed && canAttack && !isAttacking)
        //{
            //isHeavyAttacking = true;
            //Debug.Log("Heavy Attack!");
        //}
    //}
    /*
    public void BackLightAttackLeft(InputAction.CallbackContext context)
    {
        if(context.performed && canAttack && !isAttacking)
        {
            Debug.Log("Back attack! LEFT LEFT");
        }
    }
    public void BackLightAttackRight(InputAction.CallbackContext context)
    {
        if (context.performed && canAttack && !isAttacking)
        {
            Debug.Log("Back attack! Right RIGHT");
        }
    }
    */

}
