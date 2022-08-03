using System;
using UnityEngine;
using UnityEngine.InputSystem;

// AttackController requires the PlayerComponents script in order to access keybinds
[RequireComponent(typeof(PlayerComponents))]
public class AttackController : MonoBehaviour
{
    [SerializeField] private PlayerComponents playerComponentScript;

    public static AttackController instance;

    public event Action onAttackStart; //an event that occurs when the player attacks (UNUSED)
    public event Action onAttackFinish; //an event that occurs when the player is done attacking (UNUSED)

    [HideInInspector]
    public Animator animator;

    private bool isGrounded; //is the player on the ground? if so, allow them to do a standing attack

    private bool canAttack; //determines if player is allowed to attack
    private bool canBackAttack; //determines if player is allowed to do a back attack

    [HideInInspector]
    //isAttacking is true if the player successfully performs any attack
    private bool isAttacking = false;
    [HideInInspector]
    //booleans representing if the player is performing the specific attack
    public bool isLightAttacking = false;
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
        //playerComponentScript = GetComponent<PlayerComponents>();

        animator = playerComponentScript.GetAnimator();

        lightAttack = playerComponentScript.GetLightAttack();
        heavyAttack = playerComponentScript.GetHeavyAttack();

        backLightAttackLeft = playerComponentScript.GetBackLightAttackLeft();
        backLightAttackRight = playerComponentScript.GetBackLightAttackRight();

        backHeavyAttackLeft = playerComponentScript.GetBackHeavyAttackLeft();
        backHeavyAttackRight = playerComponentScript.GetBackHeavyAttackRight();

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
        //need to check if player is grounded to perform standing attacks, or not grounded for jumping attacks
        isGrounded = playerComponentScript.GetPlayerIsGrounded();


        //instead of using event systems with context, I am using .triggered and if statements to create a priority system
        // .triggered means the button was pressed

        //player must be pressing the correct bind, be allowed to attack (in other words: not mid-hurt animation), and not already attacking mid animatiom

        if (backLightAttackLeft.triggered && canBackAttack && !isBackAttacking && isGrounded)
        {
            isBackAttacking = true;

            SetPlayerIsAttacking(true);

            //Call the onAttack() event system because the player is now attacking
            PlayerHasAttackedEvent();
        }
        else if (backLightAttackRight.triggered && canBackAttack && !isBackAttacking && isGrounded)
        {
            isBackAttacking = true;

            SetPlayerIsAttacking(true);

            PlayerHasAttackedEvent();
        }

        else if (lightAttack.triggered && canAttack && !isLightAttacking && isGrounded)
        {
            isLightAttacking = true;

            SetPlayerIsAttacking(true);

            PlayerHasAttackedEvent();
        }

        else if(backHeavyAttackLeft.triggered && canBackAttack && !isBackAttacking && isGrounded)
        {
            isBackHeavyAttacking = true;

            SetPlayerIsAttacking(true);

            PlayerHasAttackedEvent();
        }

        else if (backHeavyAttackRight.triggered && canBackAttack && !isBackAttacking && isGrounded)
        {
            isBackHeavyAttacking = true;

            SetPlayerIsAttacking(true);

            PlayerHasAttackedEvent();
        }

        else if (heavyAttack.triggered && canAttack && !isHeavyAttacking && isGrounded)
        {
            isHeavyAttacking = true;

            SetPlayerIsAttacking(true);

            PlayerHasAttackedEvent();
            //Debug.Log("HEAVY ATTACK!");
        }
    }

    //Event system that occurs when the player attacks *
    public void PlayerHasAttackedEvent()
    {
        if(onAttackStart != null)
        {
            onAttackStart();
        }
    }

    public void PlayerDoneAttackingEvent()
    {
        if (onAttackFinish != null)
        {
            onAttackFinish();

        }
    }

    public void SetPlayerIsAttacking(bool boolean)
    {
        isAttacking = boolean;
    }

    public bool GetPlayerIsAttacking()
    {
        return isAttacking;
    }
        
}
