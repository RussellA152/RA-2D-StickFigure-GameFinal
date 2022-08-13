using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateBehavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if player is in idle, they're not attacking
        AttackController.instance.SetPlayerIsAttacking(false);

        //if player is in idle (or has return to idle state), they can use their equipment item
        PlayerStats.instance.SetCanUseEquipment(true);
    }
}
