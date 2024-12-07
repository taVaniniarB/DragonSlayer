using Kalagaan;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// fly �ð� ���� -> ���� ���� position down
public class FlyState : StateMachineBehaviour
{
    BoxCollider[] foots;

    float curFlyTime;
    Dragon dragon;
    BreathAttack breath;
    Transform transform;
    Transform playerTransform;
    MonoBehaviour monoBehaviour;
    
    float descentAmount = 0f;
    bool isDescenting = false;
    bool prevDescenting = false;

    float downTime = 1f;
    float downOffset = 2f;
    float pitchResetTime = 1f;

    float turnSpeed = 0.7f;
    public float maxPitch = 45f;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dragon = animator.GetComponent<Dragon>();
        breath = animator.GetComponent<BreathAttack>();
        transform = animator.transform;
        playerTransform = GameManager.Instance.playerInst.transform;

        breath.enabled = true;

        foots = dragon.foots;
        monoBehaviour = animator.GetComponent<MonoBehaviour>();

        curFlyTime = 0f;


        dragon.BreathStart();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (curFlyTime >= dragon.flyModeTime)
            isDescenting = true;

        if (isDescenting && isDescenting != prevDescenting)
        {
            DownTrigger(animator);
        }


        if (!isDescenting)
        {
            TurnToPlayer();
        }

        prevDescenting = isDescenting;

        curFlyTime += Time.deltaTime;
    }

    void TurnToPlayer()
    {
        // �÷��̾���� ���� ���
        Vector3 lookDir = (playerTransform.position - transform.position).normalized;

        // ��ǥ ȸ�� ���
        Quaternion targetRotation = Quaternion.LookRotation(lookDir);

        // ȸ�� ���� ���� ����
        Vector3 targetEulerAngles = targetRotation.eulerAngles;

        // Pitch ����
        if (targetEulerAngles.x > 180f) targetEulerAngles.x -= 360f;
        targetEulerAngles.x = Mathf.Clamp(targetEulerAngles.x, -maxPitch, maxPitch);

        Quaternion finalRotation = Quaternion.Euler(targetEulerAngles);

        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * turnSpeed);
    }


    private void DownTrigger(Animator animator)
    {
        // 1. �� �� �Ʒ��� ray�� ���� ���� ū dist�� �ϰ������� ����
        // 2. pitch ���󺹱�

        /*for (int i = 0; i < foots.Length; i++)
        {
            // ray�� ������
            Vector3 footYPos = foots[i].transform.position;

            // �� �Ʒ���(������ǥ 0, -1, 0 ����) Ray�� ���
            RaycastHit hit;
            Physics.Raycast(footYPos, Vector3.down, out hit);

            // ray�� �� ���� �� �Ÿ� ���� �ϰ������� ����
            descentAmount = Mathf.Max(descentAmount, hit.distance);
        }*/
        //Debug.Log($"�ϰ���: {descentAmount}");

        // raycast�� ���� ���, ��ǥ ���� ���� ���
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit);
        descentAmount = hit.distance;


        dragon.BreathEnd();
        curFlyTime = 0f;
        monoBehaviour.StartCoroutine(CoDown(animator, descentAmount));
        monoBehaviour.StartCoroutine(CoResetPitch(animator, pitchResetTime));
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isDescenting = false;
        prevDescenting = false;
    }

    IEnumerator CoDown(Animator animator, float amount)
    {
        float curDownTime = 0f;
        float stayY = animator.transform.position.y;
        float targetHeight = stayY - amount/* + downOffset*/;

        while (curDownTime <= downTime)
        {
            // Lerp ���� ����: ���� ��ġ���� ��ǥ ��ġ�� �̵�
            float newY = Mathf.Lerp(stayY, targetHeight, curDownTime / downTime);

            Vector3 currentPosition = animator.transform.position;
            animator.transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);

            curDownTime += Time.deltaTime;

            yield return null;
        }

        // ���� ���� �� ��ġ ����
        animator.transform.position = new Vector3(animator.transform.position.x, targetHeight, animator.transform.position.z);

        //breath.enabled = false;

        animator.SetBool("Fly", false);
        animator.SetBool("Walk", true);
    }

    IEnumerator CoResetPitch(Animator animator, float pitchResetTime)
    {
        float curTime = 0f;

        // ���� �����̼ǰ� ��ǥ �����̼� ����
        Quaternion startRotation = animator.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0f, startRotation.eulerAngles.y, startRotation.eulerAngles.z);

        while (curTime <= pitchResetTime)
        {
            // Lerp�� ����Ͽ� ���������� �����̼� ����
            animator.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, curTime / pitchResetTime);
            curTime += Time.deltaTime;
            yield return null;
        }

        // ����
        animator.transform.rotation = targetRotation;
    }
}
