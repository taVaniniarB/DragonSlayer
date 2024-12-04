using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UndeadAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform transform;
    Transform playerTransform;
    float ratio = 0f;
    float turnSpeed = 1f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        transform = animator.transform;
        playerTransform = GameManager.Instance.playerInst.transform;
        agent = animator.gameObject.GetComponent<NavMeshAgent>();
        agent.SetDestination(transform.position); // 이동을 멈추기 위해 자기 자신의 위치를 destination으로 설정
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 lookDir = Vector3.Scale((playerTransform.position - transform.position).normalized, new Vector3(1, 0, 1));
        Quaternion lookRotation = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, ratio);
        ratio += Time.deltaTime * turnSpeed;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("AttackCount", 0);
    }
}
