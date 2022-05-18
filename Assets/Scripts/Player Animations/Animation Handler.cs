using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private bool isWalking;
    [SerializeField] private Animator animator;

    private void Start()
    {
        isWalking = false;
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        Debug.Log(isWalking);
    }

    public void CheckMovement(float move, bool grounded)
    {
        if (move != 0 && grounded)
        {
            isWalking = true;
            animator.SetBool("P_Walk", isWalking);
        }

        else
        {
            isWalking = false;
            animator.SetBool("P_Walk", isWalking);
        }
    }
}
