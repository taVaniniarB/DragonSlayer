using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeOffState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Dragon>().isGround = false;
    }
}
