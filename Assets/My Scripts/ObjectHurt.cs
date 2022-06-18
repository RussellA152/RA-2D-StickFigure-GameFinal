using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHurt : MonoBehaviour, IDamageable
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnHurt(Vector3 attacker, DamageType damageType, float damage, float attackPowerX, float attackPowerY)
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(float damage, float attackPowerX, float attackPowerY)
    {
        throw new System.NotImplementedException();
    }
}
