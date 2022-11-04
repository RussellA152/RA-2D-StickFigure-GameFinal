using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour
{
    
    private bool isGrounded; // is the enemy grounded?

    [SerializeField] private Animator animator;

    [SerializeField] private BoxCollider2D groundCheckCollider; // the box collider of the ground check

    private int isGroundedHash;
    private int groundLayerInt;

    private bool disableGroundCheckCoroutineStarted; // has the disable ground check coroutine started?
    private Coroutine disableGroundCheckCoroutine; // a coroutine variable that stores the disable ground check coroutine

    private int enemyLayer;

    [SerializeField] private GameObject enemyGameObject;

    private float timerUntilUnstuck = 2f;
    private float timerStored = 2f;
    private float timeUntilCollisionTurnsBackOn = 0.5f;

    private bool collidingWithEnemy = false;

    private bool collisionTurnOnStartedCoroutine = false;

    private void Start()
    {
        // caching "Grounded" parameter from animator for performance
        isGroundedHash = Animator.StringToHash("Grounded");
        // caching "Ground" layer from inspector for performance
        groundLayerInt = LayerMask.NameToLayer("Ground");

        enemyLayer = LayerMask.NameToLayer("Enemy");
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

        // if player is colliding with "Enemy" layer and they're not grounded..
        // then collidingWithEnemy is true
        if (collision.gameObject.layer == enemyLayer && !isGrounded)
        {
            collidingWithEnemy = true;
        }
        else
        {
            collidingWithEnemy = false;
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

    // disable the enemy's ground check for a certain amount of time
    public void DisableGroundCheck(float timer)
    {
        //if there is already a coroutine going, cancel it, then start a new one
        if (disableGroundCheckCoroutineStarted)
        {
            //cancel the current disable ground check coroutine
            StopCoroutine(disableGroundCheckCoroutine);
            disableGroundCheckCoroutineStarted = false;
        }

        //set this coroutine variable to StartCoroutine()
        //this is so that we can cancel it when we need to
        disableGroundCheckCoroutine = StartCoroutine(DisableGroundCheckCoroutine(timer));
    }

    // disables ground check, then waits some time, then re-enables ground check
    private IEnumerator DisableGroundCheckCoroutine(float timer)
    {
        groundCheckCollider.enabled = false;
        yield return new WaitForSeconds(timer);
        groundCheckCollider.enabled = true;
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }


    private void Update()
    {

        if (timerUntilUnstuck <= 0f)
        {
            //collisionLayerScript.SetIgnoreEnemyLayer();
            SetIgnoreEnemyLayer();

            if (!collisionTurnOnStartedCoroutine)
                StartCoroutine(TurnCollisionBackOn(timeUntilCollisionTurnsBackOn));

        }

        if (collidingWithEnemy)
        {
            timerUntilUnstuck -= Time.deltaTime;
        }
        else
        {
            timerUntilUnstuck = timerStored;
        }
    }

    IEnumerator TurnCollisionBackOn(float timer)
    {
        collisionTurnOnStartedCoroutine = true;

        yield return new WaitForSeconds(timer);

        ResetEnemyLayer();

        collisionTurnOnStartedCoroutine = false;
    }

    private void SetIgnoreEnemyLayer()
    {
        enemyGameObject.layer = LayerMask.NameToLayer("IgnoreEnemy");
    }
    private void ResetEnemyLayer()
    {
        enemyGameObject.layer = LayerMask.NameToLayer("Enemy");
    }
}
