using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// AttackController requires the PlayerComponents script in order to access keybinds
[RequireComponent(typeof(PlayerComponents))]
public class AttackController : MonoBehaviour
{
    private PlayerComponents playerComponentScript;

    public static AttackController instance;

    public Animator animator;

    private bool canAttack; //determines if player is allowed to attack
    private bool canBackAttack; //determines if player is allowed to do a back attack

    [HideInInspector]
    //booleans representing if the player is performing the specific attack
    public bool isAttacking = false;
    [HideInInspector]
    public bool isHeavyAttacking = false;
    [HideInInspector]
    public bool isBackAttacking = false;
    [HideInInspector]
    public bool isBackHeavyAttacking = false;

    //keybindings needed to perform each attack
    private InputAction lightAttack;
    private InputAction heavyAttack;

    private InputAction backLightAttackLeft;
    private InputAction backLightAttackRight;

    private InputAction backHeavyAttackLeft;
    private InputAction backHeavyAttackRight;

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

        backHeavyAttackLeft = playerComponentScript.GetBackHeavyAttackLeft();
        backHeavyAttackRight = playerComponentScript.GetBackHeavyAttackRight();

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
            //Debug.Log("Back Attack TO THE LEFT!");
        }
        else if (backLightAttackRight.triggered && canBackAttack && !isBackAttacking)
        {
            isBackAttacking = true;
            //Debug.Log("Back Attack TO THE RIGHT!");
        }

        else if (lightAttack.triggered && canAttack && !isAttacking)
        {
            isAttacking = true;
            //Debug.Log("LIGHT ATTACK!");
        }

        else if(backHeavyAttackLeft.triggered && canBackAttack && !isBackAttacking){
            isBackHeavyAttacking = true;
        }

        else if (backHeavyAttackRight.triggered && canBackAttack && !isBackAttacking)
        {
            isBackHeavyAttacking = true;
        }

        else if (heavyAttack.triggered && canAttack && !isHeavyAttacking)
        {
            isHeavyAttacking = true;
            //Debug.Log("HEAVY ATTACK!");
        }
    }

}
