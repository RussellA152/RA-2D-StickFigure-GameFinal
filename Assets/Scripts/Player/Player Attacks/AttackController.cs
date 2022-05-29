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

    public bool isAttacking = false;
    public bool isHeavyAttacking = false;

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

        lightAttack.performed += LightAttack;
        heavyAttack.performed += HeavyAttack;
        backLightAttack.performed += BackLightAttack;
    }

    // Update is called once per frame
    void Update()
    {
        //Attack();

        canAttack = playerComponentScript.getCanAttack();
        
    }

    public void LightAttack(InputAction.CallbackContext context)
    {
        if (context.performed && canAttack &&!isAttacking)
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
        if(context.performed && canAttack)
        {
            Debug.Log("Back attack!");
        }
    }

}
