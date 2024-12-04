using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UndeadTraceState : StateMachineBehaviour
{
    public float TraceSpeed = 6f;
    NavMeshAgent agent;
    Transform playerTransform;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Wander", false);
        agent = animator.gameObject.GetComponent<NavMeshAgent>();
        agent.speed = TraceSpeed;
        playerTransform = GameManager.Instance.playerInst.transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(playerTransform.position);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
