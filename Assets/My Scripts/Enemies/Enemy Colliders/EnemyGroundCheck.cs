using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour
{
    private bool isGrounded;

    [SerializeField] private Animator animator;
    private int isGroundedHash;
    private int groundLayerInt;

    private void Start()
    {
        // caching "Grounded" parameter from animator for performance
        isGroundedHash = Animator.StringToHash("Grounded");
        // caching "Ground" layer from inspector for performance
        groundLayerInt = LayerMask.NameToLayer("Ground");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == groundLayerInt)
        {
            isGrounded = true;
            animator.SetBool(isGroundedHash, true);
        }
        //else
        //{
            //isGrounded = false;
            //animator.SetBool(isGroundedHash, false);
        //}
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == groundLayerInt)
        {
            isGrounded = true;
            animator.SetBool(isGroundedHash, true);
        }
        //else
        //{
            //isGrounded = false;
            //animator.SetBool(isGroundedHash, false);
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == groundLayerInt)
        {
            isGrounded = false;
            animator.SetBool(isGroundedHash, false);
        }
    }
}
