using UnityEngine;
using UnityEngine.AI;
public class UndeadWanderState : StateMachineBehaviour
{
    public float wanderSpeed = 3.5f;
    Transform transform;
    float posUpdateDelayTime = 3f; // 위치 업데이트 딜레이
    UnityEngine.AI.NavMeshAgent agent;
    float curTime;
    float range = 20f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        curTime = posUpdateDelayTime;
        agent = animator.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = wanderSpeed;
        transform = animator.transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (curTime >= posUpdateDelayTime)
        {
            Vector3 randomPos = GetRandomPos(); // NavMesh 위의 랜덤한 위치를 가져와 목표로 세팅
            agent.SetDestination(randomPos);
            posUpdateDelayTime = Random.Range(3, 5);
            curTime = 0f;
        }

        curTime += Time.deltaTime;
    }
    Vector3 GetRandomPos()
    {
        Vector3 randomDir = Random.insideUnitSphere * range; // 범위 내 랜덤한 방향 벡터
        Vector3 randomPos = randomDir + transform.position;

        // range 내 randomPos와 가장 가까운 위치 반환
        if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, range, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return transform.position; // NavMesh 위의 랜덤 위치를 찾지 못한 경우 -> 현재 위치 반환
        }
    }
}
