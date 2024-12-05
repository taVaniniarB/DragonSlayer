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
    Dragon dragon;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dragon = animator.GetComponent<Dragon>();
        dragon.BreathEnd();

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
        transform.position = new Vector3(transform.position.x, (transform.position.y - (fallSpeed * Time.deltaTime)), transform.position.z);

        if(transform.position.y <= targetHeight)
        {
            animator.transform.position = new Vector3(animator.transform.position.x, targetHeight, animator.transform.position.z);
            animator.SetTrigger("FallEnd");
        }
    }
}
