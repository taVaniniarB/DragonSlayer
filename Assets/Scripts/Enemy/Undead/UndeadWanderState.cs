using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class UndeadWanderState : StateMachineBehaviour
{
    public float wanderSpeed = 3.5f;
    Transform transform;
    float updateInterval = 3f; // 위치 업데이트 간격
    UnityEngine.AI.NavMeshAgent agent;
    float curTime;
    float range = 20f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        curTime = updateInterval;
        agent = animator.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = wanderSpeed;
        transform = animator.transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (curTime >= updateInterval)
        {
            Vector3 randomPosition = GetRandomPositionOnNavMesh(); // NavMesh 위의 랜덤한 위치를 가져와 목표로 세팅
            agent.SetDestination(randomPosition);
            updateInterval = Random.Range(3, 5);
            curTime = 0f;
        }

        curTime += Time.deltaTime;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    Vector3 GetRandomPositionOnNavMesh()
    {
        Vector3 randomDir = Random.insideUnitSphere * range; // 범위 내 랜덤한 방향 벡터
        randomDir += transform.position; // 랜덤 방향 벡터를 현재 위치에 더하기

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDir, out hit, range, NavMesh.AllAreas)) // 랜덤 위치가 NavMesh 위에 있는가?
        {
            return hit.position; // NavMesh 위의 랜덤 위치 반환
        }
        else
        {
            return transform.position; // NavMesh 위의 랜덤 위치를 찾지 못한 경우 > 현재 위치 반환
        }
    }
}
