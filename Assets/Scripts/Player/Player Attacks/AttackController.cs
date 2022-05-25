using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    private PlayerComponents playerComponentScript;

    public static AttackController instance;

    public Animator animator;


    public bool isAttacking = false;
    public bool isHeavyAttacking = false;

    private bool canInteract; //determines if player is allowed to attack

    private void Awake()
    {
        instance = this;
    
    }

    // Start is called before the first frame update
    void Start()
    {
        playerComponentScript = GetComponent<PlayerComponents>();
        animator = playerComponentScript.getAnimator();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();

        canInteract = playerComponentScript.getCanInteract();
        
    }

    public void Attack()
    {
        if(Input.GetMouseButtonDown(0) && !isAttacking && canInteract)
        {
            isAttacking = true;
        }

        if(Input.GetMouseButtonDown(1) && !isHeavyAttacking && canInteract)
        {
            isHeavyAttacking = true;
        }
    }
}
