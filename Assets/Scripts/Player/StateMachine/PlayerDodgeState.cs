using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.applyRootMotion = true;
        animator.GetComponent<PlayerStat>().godMode = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.applyRootMotion = false;
        animator.GetComponent<PlayerStat>().godMode = false;
    }
}
