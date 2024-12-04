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
    float ratio = 0f;
    MonoBehaviour monoBehaviour;
    
    float descentAmount = 0f;
    bool isDescenting = false;
    bool prevDescenting = false;
    float fireIntensity = 0;
    float fireDecSpeed = 3f;
    float turnSpeed = 0.5f;

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

        
        if (isDescenting)
        {
            //Debug.Log("�ϰ���");
            fireIntensity -= Time.deltaTime * fireDecSpeed;
        }
        else
        {
            //Debug.Log("flying");
            fireIntensity += Time.deltaTime;
            RotateToPlayer();
        }

        prevDescenting = isDescenting;

        //dragon.isBreathing = fireIntensity;

        curFlyTime += Time.deltaTime;
    }

    void RotateToPlayer()
    {
        Vector3 lookDir = Vector3.Scale((playerTransform.position - transform.position).normalized, new Vector3(1, 0, 1));
        Quaternion lookRotation = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, ratio);
        ratio += Time.deltaTime * turnSpeed;
    }

    private void DownTrigger(Animator animator)
    {
        // �� �� �Ʒ��� ray�� ���� ���� ū dist�� �ϰ������� ����

        for (int i = 0; i < foots.Length; i++)
        {
            // ray�� ������
            Vector3 footYPos = foots[i].transform.position;

            // �� �Ʒ���(������ǥ 0, -1, 0 ����) Ray�� ���
            RaycastHit hit;
            Physics.Raycast(footYPos, Vector3.down, out hit);

            // ray�� �� ���� �� �Ÿ� ���� �ϰ������� ����
            descentAmount = Mathf.Max(descentAmount, hit.distance);
        }
        //Debug.Log($"�ϰ���: {descentAmount}");
        dragon.BreathEnd();
        curFlyTime = 0f;
        monoBehaviour.StartCoroutine(CoDown(animator, descentAmount));
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isDescenting = false;
        prevDescenting = false;
    }

    IEnumerator CoDown(Animator animator, float amount)
    {
        float curDownTime = 0f;
        float downTime = 1f;
        float stayY = animator.transform.position.y;
        float targetHeight = stayY - amount + 2;

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

        breath.enabled = false;
        // Fly �ִϸ��̼� ���� ����
        animator.SetBool("Fly", false);
        animator.SetBool("Walk", true);
    }
}