using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this class is meant to change the layers of the player's colliders on command
// mainly used for attacks that send player into the air and wouldn't want collider to block player's momentum
// ex. combo 3 heavy will send player into a flying knee, but collider will block player's upward force when it touches an enemy, so we turn it to a different layer for a brief moment
public class PlayerCollisionLayerChange : MonoBehaviour
{
    //[SerializeField] private BoxCollider2D mainCollider;
    [SerializeField] private CircleCollider2D headCollider;
    [SerializeField] private CircleCollider2D legCollider;

    private int ignoreEnemyLayerHash;
    private int playerLayerHash;

    private void Start()
    {
        // cache layer names for performance (this could happen often depending on player attack frequency)
        ignoreEnemyLayerHash = LayerMask.NameToLayer("IgnoreEnemy");
        playerLayerHash = LayerMask.NameToLayer("Player");
    }

    public void SetIgnoreEnemyLayer()
    {
        //mainCollider.gameObject.layer = LayerMask.NameToLayer("IgnoreEnemy");
        headCollider.gameObject.layer = ignoreEnemyLayerHash;
        legCollider.gameObject.layer = ignoreEnemyLayerHash;
    }

    public void ResetLayer()
    {
        //mainCollider.gameObject.layer = LayerMask.NameToLayer("Player");
        headCollider.gameObject.layer = playerLayerHash;
        legCollider.gameObject.layer = playerLayerHash;
    }
}
