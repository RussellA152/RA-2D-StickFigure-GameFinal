using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionLayerChange : MonoBehaviour
{
    [SerializeField] private CircleCollider2D headCollider;
    [SerializeField] private CircleCollider2D legCollider;

    public void SetIgnoreEnemyLayer()
    {
        headCollider.gameObject.layer = LayerMask.NameToLayer("IgnoreEnemy");
        legCollider.gameObject.layer = LayerMask.NameToLayer("IgnoreEnemy");
    }

    public void ResetLayer()
    {
        headCollider.gameObject.layer = LayerMask.NameToLayer("Player");
        legCollider.gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
