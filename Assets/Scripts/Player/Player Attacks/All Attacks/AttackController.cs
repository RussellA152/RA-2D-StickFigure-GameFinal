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
    private InputAction backLightAttack;

    

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
        animator = playerComponentScript.getAnimator();

        lightAttack = playerComponentScript.getLightAttack();
        heavyAttack = playerComponentScript.getHeavyAttack();
        backLightAttack = playerComponentScript.getBackLightAttack();

        //lightAttack.performed += LightAttack;
        //heavyAttack.performed += HeavyAttack;
        //backLightAttack.performed += BackLightAttack;
    }

    // Update is called once per frame
    void Update()
    {
        
        canAttack = playerComponentScript.getCanAttack();
        canBackAttack = playerComponentScript.getCanBackAttack();

        Attack();



    }
    private void Attack()
    {
        //instead of using event systems with context, I am using .triggered and if statements to create a priority system
        if (backLightAttack.triggered && canBackAttack && !isBackAttacking)
        {
            isBackAttacking = true;
            //Debug.Log("BACK ATTACK!");
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

    /*
    public void LightAttack(InputAction.CallbackContext context)
    {
        if (context.performed && canAttack &&!isAttacking && !isBackAttacking)
        {
            isAttacking = true;
            Debug.Log("Light Attack!");
        }
    }

    public void HeavyAttack(InputAction.CallbackContext context)
    {
        if (context.performed && canAttack && !isAttacking)
        {
            isHeavyAttacking = true;
            Debug.Log("Heavy Attack!");
        }
    }

    public void BackLightAttack(InputAction.CallbackContext context)
    {
        if(context.performed && canAttack && !isAttacking)
        {
            isBackAttacking = true;
            Debug.Log("Back attack!");
        }
    }

    */

}
