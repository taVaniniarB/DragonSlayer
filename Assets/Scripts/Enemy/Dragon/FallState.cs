using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : StateMachineBehaviour
{
    float fallAmount = 0f;
    float targetHeight = 0f;
    float stayY = 0f;

    float fallSpeed = 5f;
    Transform transform;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        transform = animator.transform;
        stayY = transform.position.y;
        // raycast로 높이 계산, 목표 도달 높이 계산
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit);
        fallAmount = hit.distance;
        targetHeight = stayY - fallAmount;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*float newY = Mathf.Lerp(stayY, targetHeight, curDownTime / downTime);

        Vector3 currentPosition = animator.transform.position;
        animator.transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);

        curDownTime += Time.deltaTime;

        // fall 종료
        if (curDownTime > downTime)
        {
            //위치 보정
            animator.transform.position = new Vector3(animator.transform.position.x, targetHeight, animator.transform.position.z);
            animator.SetTrigger("FallEnd");
        }*/

        transform.position = new Vector3(transform.position.x, (transform.position.y - (fallSpeed * Time.deltaTime)), transform.position.z);

        if(transform.position.y <= targetHeight)
        {
            animator.transform.position = new Vector3(animator.transform.position.x, targetHeight, animator.transform.position.z);
            animator.SetTrigger("FallEnd");
        }
    }
}
