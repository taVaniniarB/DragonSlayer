using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeState : StateMachineBehaviour
{
    Transform playerTransform;
    Transform transform;
    Dragon dragon;
    ChargeAttack charge;
    float curTime = 0f;
    Vector3 dir;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerTransform = GameManager.Instance.playerInst.transform;
        transform = animator.GetComponent<Transform>();
        dragon = animator.GetComponent<Dragon>();
        charge = animator.GetComponent<ChargeAttack>();
        charge.isCharging = true;

        // 돌진 방향 세팅
        Vector3 dir = (playerTransform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir);
        animator.SetBool("Walk", false);
        animator.SetBool("Run", true);
        Debug.Log("돌진 방향 세팅 완료");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float dt = Time.deltaTime;

        transform.position += transform.forward * dt * dragon.GroundChargeSpeed;
        
        if (curTime > dragon.groundChargeTime)
        {
            animator.SetBool("Run", false);
            animator.SetBool("Walk", true);
            curTime = 0f;
        }
        curTime += dt;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        charge.isCharging = false;
    }
}
