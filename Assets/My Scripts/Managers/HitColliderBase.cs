using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitColliderBase : MonoBehaviour
{

    protected abstract void OnTriggerEnter2D(Collider2D collision);

    protected abstract void DealDamage(float damage, float attackPowerX, float attackPowerY);

    public abstract void UpdateAttackValues(DamageType damageTypeParameter, float damage, float attackPowerX, float attackPowerY);

}
