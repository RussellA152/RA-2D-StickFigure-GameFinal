using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstuckCollisionPlayer : MonoBehaviour
{
    [SerializeField] private PlayerComponents playerCompScript;
    [SerializeField] private PlayerCollisionLayerChange collisionLayerScript;

    private bool isGrounded;

    private int enemyLayer;

    private float timerUntilUnstuck = 2f;
    private float timerStored = 2f;
    private float timeUntilCollisionTurnsBackOn = 0.75f;

    private bool collidingWithEnemy = false;

    private bool startedCoroutine = false;




    private void Start()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
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
    }

    private void Update()
    {
        // update player's "isGrounded"
        isGrounded = playerCompScript.GetPlayerIsGrounded();

        if(timerUntilUnstuck <= 0f)
        {
            collisionLayerScript.SetIgnoreEnemyLayer();

            if (!startedCoroutine)
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
        Debug.Log("UNSTUCK ME!");
        startedCoroutine = true;

        yield return new WaitForSeconds(timer);

        collisionLayerScript.ResetLayer();

        startedCoroutine = false;
    }
}
