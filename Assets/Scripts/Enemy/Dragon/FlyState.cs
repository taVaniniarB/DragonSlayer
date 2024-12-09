using Kalagaan;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// fly 시간 도달 -> 착지 위해 position down
public class FlyState : StateMachineBehaviour
{
    float curFlyTime;
    Dragon dragon;
    BreathAttack breath;
    Transform transform;
    Transform playerTransform;
    MonoBehaviour monoBehaviour;

    float targetHeight;
    bool isDescenting = false;
    bool prevDescenting = false;

    float downTime = 1f;
    float pitchResetTime = 1f;

    float turnSpeed = 0.8f;
    public float maxPitch = 45f;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dragon = animator.GetComponent<Dragon>();
        breath = animator.GetComponent<BreathAttack>();
        transform = animator.transform;
        playerTransform = GameManager.Instance.playerInst.transform;

        breath.enabled = true;

        monoBehaviour = animator.GetComponent<MonoBehaviour>();

        curFlyTime = 0f;


        dragon.BreathStart();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // flyModeTime 시간 도달
        if (curFlyTime >= dragon.flyModeTime)
            isDescenting = true;
        
        // 하강 시작
        if (isDescenting && isDescenting != prevDescenting)
            DownTrigger(animator);


        if (!isDescenting)
            TurnToPlayer();

        prevDescenting = isDescenting;

        curFlyTime += Time.deltaTime;
    }

    void TurnToPlayer()
    {
        // 방향을 회전으로 변환
        Vector3 lookDir = (playerTransform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(lookDir);
        Vector3 targetEuler = targetRotation.eulerAngles;

        // 회전의 Pitch 제한
        targetEuler.x = Mathf.Clamp(targetEuler.x, -maxPitch, maxPitch);

        Quaternion finalRotation = Quaternion.Euler(targetEuler);

        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * turnSpeed);
    }


    private void DownTrigger(Animator animator)
    {
        // raycast로 높이 계산, 목표 도달 높이 계산
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit);
        targetHeight = hit.point.y;


        dragon.BreathEnd();
        curFlyTime = 0f;
        monoBehaviour.StartCoroutine(CoDown(animator, targetHeight));
        monoBehaviour.StartCoroutine(CoResetPitch(animator, pitchResetTime));
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isDescenting = false;
        prevDescenting = false;
    }

    IEnumerator CoDown(Animator animator, float targetHeight)
    {
        float curDownTime = 0f;
        float stayY = animator.transform.position.y;

        while (curDownTime <= downTime)
        {
            float newY = Mathf.Lerp(stayY, targetHeight, curDownTime / downTime);

            Vector3 currentPosition = animator.transform.position;
            animator.transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);

            curDownTime += Time.deltaTime;

            yield return null;
        }

        // 루프 종료 후 위치 보정
        animator.transform.position = new Vector3(animator.transform.position.x, targetHeight, animator.transform.position.z);

        animator.SetBool("Fly", false);
        animator.SetBool("Walk", true);
    }

    IEnumerator CoResetPitch(Animator animator, float pitchResetTime)
    {
        float curTime = 0f;

        Quaternion startRotation = animator.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0f, startRotation.eulerAngles.y, startRotation.eulerAngles.z);

        while (curTime <= pitchResetTime)
        {
            animator.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, curTime / pitchResetTime);
            curTime += Time.deltaTime;
            yield return null;
        }

        // 보정
        animator.transform.rotation = targetRotation;
    }
}
