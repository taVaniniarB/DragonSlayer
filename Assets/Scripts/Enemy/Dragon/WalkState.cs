using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾ ���� ���� ���� ������ Bite

// enter ��� �÷��̾��� ��ǥ�� �����صΰ�
// �� ��ǥ �߽����� ���� �׸��� �̵� (Slerp)
// walk ���� ���� ���� �Ÿ��� Ȯ���Ǿ�� ��
// bite -> run(�ݴ�������� �̵�) -> walk
public class WalkState : StateMachineBehaviour
{
    Transform playerTransform;
    Transform transform;
    Transform headTransform;
    Vector3 origin;
    //int dirSign = 1; // �ݽð�: 1 / �ð�: -1
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
        if (!animator.GetBool("Walk")) // �ٸ� �ִϸ��̼����� ���� ���� ����Ǵ� ���� ����
            return;

        // origin �߽����� ���� �׸��� �̵�
        MoveCircle();
    }

    private void MoveCircle()
    {
        // ���� 2���� ȸ�� ��ȯ
        // https://satlab.tistory.com/91
        
        Vector3 curPos = transform.position;

        // ������������ �����ġ
        Vector3 offset = curPos - origin;

        float radius = offset.magnitude; // ���� ������

        if (radius <= 1e-2) return; // �������� �ʹ� ������ �н�

        // �� ������ ���� ������ ����
        float angle = (dragon.walkSpeed * Time.deltaTime) / radius;

        // ��� ����
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);
        float newX = (offset.x * cos) - (offset.z * sin);
        float newZ = (offset.x * sin) + (offset.z * cos);

        Vector3 newOffset = new Vector3(newX, offset.y, newZ);
        Vector3 newPos = origin + newOffset;
        transform.position = newPos;

        // �巡���� �ٶ󺸴� ���� ����
        Vector3 forward = (newPos - curPos).normalized;
        transform.rotation = Quaternion.LookRotation(forward);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
