using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour
{
    private bool isGrounded;

    [SerializeField] private Animator animator;
    private int isGroundedHash;

    private void Start()
    {
        isGroundedHash = Animator.StringToHash("Grounded");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
            animator.SetBool(isGroundedHash, true);
        }
        else
        {
            isGrounded = false;
            animator.SetBool(isGroundedHash, false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
            animator.SetBool(isGroundedHash, false);
        }
    }
}
