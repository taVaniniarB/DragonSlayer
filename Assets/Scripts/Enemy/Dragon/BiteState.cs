using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Run", true);
        animator.SetBool("Walk", false);
        animator.SetBool("Bite", false);
    }
}
