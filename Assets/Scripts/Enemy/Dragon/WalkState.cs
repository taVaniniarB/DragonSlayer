using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : StateMachineBehaviour
{
    Transform playerTransform;
    Transform transform;
    Vector3 origin;
    Dragon dragon;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerTransform = GameManager.Instance.playerInst.transform;
        transform = animator.GetComponent<Transform>();
        dragon = animator.GetComponent<Dragon>();

        origin = playerTransform.position;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.GetBool("Walk")) // 다른 애니메이션으로 블랜딩 도중 실행되는 것을 방지
            return;

        // origin 중심으로 원을 그리며 이동
        MoveCircle();
    }

    private void MoveCircle()
    {
        // 점의 2차원 회전 변환
        
        Vector3 curPos = transform.position;

        // 원점에서부터 상대위치
        Vector3 offset = curPos - origin;

        float radius = offset.magnitude; // 원의 반지름

        // 한 프레임 동안 움직일 각도
        float angle = (dragon.walkSpeed * Time.deltaTime) / radius;

        // 행렬 적용
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);
        float newX = (offset.x * cos) - (offset.z * sin);
        float newZ = (offset.x * sin) + (offset.z * cos);

        Vector3 newOffset = new Vector3(newX, offset.y, newZ);
        Vector3 newPos = origin + newOffset;
        transform.position = newPos;

        // 드래곤이 바라보는 방향 설정
        Vector3 forward = (newPos - curPos).normalized;
        transform.rotation = Quaternion.LookRotation(forward);
    }
}
