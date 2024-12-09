using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadIdleState : StateMachineBehaviour
{
    StateController controller;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.gameObject.GetComponent<StateController>();
        controller.TriggerStateWithDelay("Idle", "Wander", controller.idleTime);
    }
}
