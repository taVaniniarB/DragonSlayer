using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : StateMachineBehaviour
{
    float targetHeight = 0f;

    float fallSpeed = 5f;
    Transform transform;
    Dragon dragon;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dragon = animator.GetComponent<Dragon>();
        dragon.BreathEnd();

        transform = animator.transform;

        SetTargetHeight();
    }

    private void SetTargetHeight()
    {
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit);
        targetHeight = hit.point.y;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float newY = transform.position.y - (fallSpeed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        if(transform.position.y <= targetHeight)
        {
            animator.transform.position = new Vector3(animator.transform.position.x, targetHeight, animator.transform.position.z);
            animator.SetTrigger("FallEnd");
        }
    }
}
