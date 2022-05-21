using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public static AttackController instance;
    public Animator animator;
    public bool isAttacking = false;
    public bool isHeavyAttacking = false;

    private void Awake()
    {
        instance = this;
    
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        
    }

    public void Attack()
    {
        if(Input.GetMouseButtonDown(0) && !isAttacking)
        {
            isAttacking = true;
        }

        if(Input.GetMouseButtonDown(1) && !isHeavyAttacking)
        {
            isHeavyAttacking = true;
        }
    }
}
