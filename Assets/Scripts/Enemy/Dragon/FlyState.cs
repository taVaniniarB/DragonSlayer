using Kalagaan;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// fly 시간 도달 -> 착지 위해 position down
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
        // 플레이어와의 방향 계산
        Vector3 lookDir = (playerTransform.position - transform.position).normalized;

        // 목표 회전 계산
        Quaternion targetRotation = Quaternion.LookRotation(lookDir);

        // 회전 각도 제한 적용
        Vector3 targetEulerAngles = targetRotation.eulerAngles;

        // Pitch 제한
        if (targetEulerAngles.x > 180f) targetEulerAngles.x -= 360f;
        targetEulerAngles.x = Mathf.Clamp(targetEulerAngles.x, -maxPitch, maxPitch);

        Quaternion finalRotation = Quaternion.Euler(targetEulerAngles);

        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * turnSpeed);
    }


    private void DownTrigger(Animator animator)
    {
        // 1. 네 발 아래로 ray를 쏴서 가장 큰 dist를 하강량으로 저장
        // 2. pitch 원상복귀

        /*for (int i = 0; i < foots.Length; i++)
        {
            // ray의 시작점
            Vector3 footYPos = foots[i].transform.position;

            // 발 아래로(월드좌표 0, -1, 0 방향) Ray를 쏜다
            RaycastHit hit;
            Physics.Raycast(footYPos, Vector3.down, out hit);

            // ray들 중 가장 긴 거리 값을 하강량으로 저장
            descentAmount = Mathf.Max(descentAmount, hit.distance);
        }*/
        //Debug.Log($"하강량: {descentAmount}");

        // raycast로 높이 계산, 목표 도달 높이 계산
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
            // Lerp 방향 수정: 시작 위치에서 목표 위치로 이동
            float newY = Mathf.Lerp(stayY, targetHeight, curDownTime / downTime);

            Vector3 currentPosition = animator.transform.position;
            animator.transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);

            curDownTime += Time.deltaTime;

            yield return null;
        }

        // 루프 종료 후 위치 보정
        animator.transform.position = new Vector3(animator.transform.position.x, targetHeight, animator.transform.position.z);

        //breath.enabled = false;

        animator.SetBool("Fly", false);
        animator.SetBool("Walk", true);
    }

    IEnumerator CoResetPitch(Animator animator, float pitchResetTime)
    {
        float curTime = 0f;

        // 현재 로테이션과 목표 로테이션 설정
        Quaternion startRotation = animator.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0f, startRotation.eulerAngles.y, startRotation.eulerAngles.z);

        while (curTime <= pitchResetTime)
        {
            // Lerp를 사용하여 점진적으로 로테이션 복구
            animator.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, curTime / pitchResetTime);
            curTime += Time.deltaTime;
            yield return null;
        }

        // 보정
        animator.transform.rotation = targetRotation;
    }
}
