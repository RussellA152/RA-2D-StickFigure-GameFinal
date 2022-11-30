using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IDamageDealingItem is the interface that all "damage" items can implement
//unlike IDamageDealingCharacter, this interface does not contain UpdateAttackValues because items will not have attack animations that
//have different values of damages like the Player and Enemy do
public interface IDamageDealingItem : IDamageDealing
{

    public void ShakeScreen();
}
