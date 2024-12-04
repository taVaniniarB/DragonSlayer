using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.applyRootMotion = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.applyRootMotion = false;
/*
        if(!animator.GetBool("ExitAttack"))
            animator.GetComponent<PlayerAttack>().ExitAttack();*/
    }
}