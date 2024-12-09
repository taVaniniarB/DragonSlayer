using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.applyRootMotion = true;
        animator.GetComponent<PlayerState>().godMode = true;
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.applyRootMotion = false;
        animator.GetComponent<PlayerState>().godMode = false;
    }
}
