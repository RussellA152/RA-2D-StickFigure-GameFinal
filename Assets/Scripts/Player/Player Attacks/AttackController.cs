using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public static AttackController instance;
    public Animator animator;
    public bool isAttacking = false;

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
        if(Input.GetButtonDown("Fire1") && !isAttacking)
        {
            isAttacking = true;
        }
    }
}
