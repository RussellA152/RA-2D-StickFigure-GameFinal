using System;
using System.Collections;
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
    private bool canJumpAttack;
    private bool canJumpHeavyAttack;
    private bool canGroundSlam;

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

    [HideInInspector]
    public bool isJumpLightAttacking = false;

    [HideInInspector]
    public bool isJumpHeavyAttacking = false;

    //keybindings needed to perform each attack
    private InputAction lightAttack;
    private InputAction heavyAttack;

    private InputAction backLightAttackLeft;
    private InputAction backLightAttackRight;

    private InputAction backHeavyAttackLeft;
    private InputAction backHeavyAttackRight;

    [Header("Attack Timers")]
    [SerializeField] private float backAttackTimer; // when player turns around, how much time do they have to perform a back attack (once this hits 0, the player must turn around again to perform a back attack)
    private float backAttackTimerStored; // a float variable that remembers the original value of the backAttackTimer (we need this because backAttackTimer is modified during gameplay, and need to reset it often)
    [SerializeField] private float jumpHeavyAttackCooldown; // when player performs a jump heavy attack, how long do they need to wait to perform it again?
    private float jumpHeavyAttackCooldownStored; // a float variable that remembers the original value of the jump heavy attack cooldown (we need this because backAttackTimer is modified during gameplay, and need to reset it often)

    private Coroutine jumpHeavyAttackCooldownCoroutine;
    private bool jumpHeavyAttackCooldownStarted;

    private int groundSlamHash;

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

        backAttackTimerStored = backAttackTimer;
        jumpHeavyAttackCooldownStored = jumpHeavyAttackCooldown;

        groundSlamHash = Animator.StringToHash("canGroundSlam");

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
        canJumpAttack = playerComponentScript.GetCanJumpAttack();
        canJumpHeavyAttack = playerComponentScript.GetCanJumpHeavyAttack();

        Attack();

        // back attack timer is always counting down
        // decrement back attack timer until it hits 0 (player can no longer back attack)
        if (backAttackTimer > 0f)
            backAttackTimer -= Time.deltaTime;
        else
            backAttackTimer = 0f;

        CheckBackAttack();

        //Debug.Log("can jump attack?: " + canJumpAttack);
        //Debug.Log("can jump attack? player comp: " + playerComponentScript.GetCanJumpAttack());


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

        else if(lightAttack.triggered && canJumpAttack && !isJumpLightAttacking && !isGrounded)
        {
            isJumpLightAttacking = true;
            SetPlayerIsAttacking(true);
            PlayerHasAttackedEvent();
        }

        else if (heavyAttack.triggered && canJumpHeavyAttack && !isJumpHeavyAttacking && !isGrounded)
        {
            isJumpHeavyAttacking = true;
            SetPlayerIsAttacking(true);
            PlayerHasAttackedEvent();

            //if (!jumpHeavyAttackCooldownStarted)
                //StartCoroutine(StartJumpHeavyAttackCooldown(jumpHeavyAttackCooldown));

            //if (jumpHeavyAttackCooldownStarted)
            //{
            //    //cancel the current transition coroutine
            //    StopCoroutine(jumpHeavyAttackCooldownCoroutine);
            //    jumpHeavyAttackCooldownStarted = false;
            //}

            //set this coroutine variable to StartCoroutine()
            //this is so that we can cancel it when we need to
            //jumpHeavyAttackCooldownCoroutine = StartCoroutine(StartJumpHeavyAttackCooldown(jumpHeavyAttackCooldown));
        }
    }

    //Event system that occurs when the player attacks * (doesn't have to land)
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

    public void CheckBackAttack()
    {
        //if back attack timer reaches 0, then the player must turn around again
        if (backAttackTimer <= 0f)
            playerComponentScript.SetCanBackAttack(false);
        //Debug.Log(backAttackTimer);
    }

    public void ResetBackAttackTimer()
    {
        backAttackTimer = backAttackTimerStored;
        playerComponentScript.SetCanBackAttack(true);
    }

    //IEnumerator StartJumpHeavyAttackCooldown(float cooldown)
    //{
    //    playerComponentScript.SetCanJumpHeavyAttack(false);

    //    jumpHeavyAttackCooldownStarted = true;

    //    yield return new WaitForSeconds(cooldown);

    //    jumpHeavyAttackCooldownStarted = false;

    //    jumpHeavyAttackCooldown = jumpHeavyAttackCooldownStored;
    //    playerComponentScript.SetCanJumpHeavyAttack(true);
    //}


}
