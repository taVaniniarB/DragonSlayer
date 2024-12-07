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

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerTransform = GameManager.Instance.playerInst.transform;
        transform = animator.GetComponent<Transform>();
        dragon = animator.GetComponent<Dragon>();
        charge = animator.GetComponent<ChargeAttack>();
        charge.isCharging = true;
        animator.SetBool("Walk", false);
        animator.SetBool("Run", true);

        // 돌진 방향 세팅
        SetChargeDir();
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        transform.position += transform.forward * Time.deltaTime * dragon.GroundChargeSpeed;

        ChargeTimeCheck(animator);
    }

    private void SetChargeDir()
    {
        Vector3 dir = (playerTransform.position - transform.position);
        dir = Vector3.Scale(dir, new Vector3(1, 0, 1)).normalized;
        transform.rotation = Quaternion.LookRotation(dir);
        Debug.Log("돌진 방향 세팅 완료");
    }
    private void ChargeTimeCheck(Animator animator)
    {
        if (curTime >= dragon.groundChargeTime)
        {
            animator.SetBool("Run", false);
            animator.SetBool("Walk", true);
            curTime = 0f;
        }
        curTime += Time.deltaTime;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        charge.isCharging = false;
    }
}
