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
    float ratio = 0f;
    MonoBehaviour monoBehaviour;
    
    float descentAmount = 0f;
    bool isDescenting = false;
    bool prevDescenting = false;
    float fireIntensity = 0;
    float fireDecSpeed = 3f;
    float turnSpeed = 0.5f;

    public float constraintPitch = 45f;

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


        dragon.isGround = false;
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
            //Debug.Log("하강중");
            fireIntensity -= Time.deltaTime * fireDecSpeed;
        }
        else
        {
            //Debug.Log("flying");
            fireIntensity += Time.deltaTime;
            TurnToPlayer();
            //PitchToPlayer();
        }

        prevDescenting = isDescenting;

        //dragon.isBreathing = fireIntensity;

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
        targetEulerAngles.x = Mathf.Clamp(targetEulerAngles.x, -constraintPitch, constraintPitch);

        Quaternion finalRotation = Quaternion.Euler(targetEulerAngles);

        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * turnSpeed);
    }


    private void DownTrigger(Animator animator)
    {
        // 1. 네 발 아래로 ray를 쏴서 가장 큰 dist를 하강량으로 저장
        // 2. pitch 원상복귀

        for (int i = 0; i < foots.Length; i++)
        {
            // ray의 시작점
            Vector3 footYPos = foots[i].transform.position;

            // 발 아래로(월드좌표 0, -1, 0 방향) Ray를 쏜다
            RaycastHit hit;
            Physics.Raycast(footYPos, Vector3.down, out hit);

            // ray들 중 가장 긴 거리 값을 하강량으로 저장
            descentAmount = Mathf.Max(descentAmount, hit.distance);
        }
        //Debug.Log($"하강량: {descentAmount}");
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
            // Lerp 방향 수정: 시작 위치에서 목표 위치로 이동
            float newY = Mathf.Lerp(stayY, targetHeight, curDownTime / downTime);

            Vector3 currentPosition = animator.transform.position;
            animator.transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);

            curDownTime += Time.deltaTime;

            yield return null;
        }

        // 루프 종료 후 위치 보정
        animator.transform.position = new Vector3(animator.transform.position.x, targetHeight, animator.transform.position.z);

        breath.enabled = false;
        // Fly 애니메이션 종료 설정
        animator.SetBool("Fly", false);
        animator.SetBool("Walk", true);
    }
}
