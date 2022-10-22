using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionLayerChange : MonoBehaviour
{
    //[SerializeField] private BoxCollider2D mainCollider;
    [SerializeField] private CircleCollider2D headCollider;
    [SerializeField] private CircleCollider2D legCollider;

    public void SetIgnoreEnemyLayer()
    {
        //mainCollider.gameObject.layer = LayerMask.NameToLayer("IgnoreEnemy");
        headCollider.gameObject.layer = LayerMask.NameToLayer("IgnoreEnemy");
        legCollider.gameObject.layer = LayerMask.NameToLayer("IgnoreEnemy");
    }

    public void ResetLayer()
    {
        //mainCollider.gameObject.layer = LayerMask.NameToLayer("Player");
        headCollider.gameObject.layer = LayerMask.NameToLayer("Player");
        legCollider.gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
