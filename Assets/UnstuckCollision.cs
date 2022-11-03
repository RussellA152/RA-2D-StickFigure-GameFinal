using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstuckCollision : MonoBehaviour
{
    private bool isGrounded;

    private int enemyLayer;

    private float timerUntilUnstuck = 3f;


    private void Start()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == enemyLayer && !isGrounded)
        {
            //Debug.Log()
        }
    }
}
