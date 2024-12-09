using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathAttack : MonoBehaviour
{
    public PlayerState target;
    public float damagePerTick = 1.5f;
    public float damageDelay = 0.3f;
    bool prevPlayerBurning = false;
    Coroutine damageCoroutine = null;

    void Update()
    {
        // isBurning이 true가 되어 파티클 범위 내에 진입 시작 -> 화염 대미지 코루틴 시작
        if (target.isBurning && !prevPlayerBurning)
        {
            // 코루틴이 중복 호출되는 현상 방지
            if (damageCoroutine == null)
                damageCoroutine = StartCoroutine(CoFireDamage());
        }
        // 이번 프레임에서 파티클 범위를 탈출 -> 코루틴 중단
        else if (!target.isBurning && prevPlayerBurning)
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }

        prevPlayerBurning = target.isBurning;
    }


    // delay 초마다 한 번 대미지를 주는 코루틴
    IEnumerator CoFireDamage()
    {
        while (target.isBurning)
        {
            ApplyDamage(damagePerTick);
            yield return new WaitForSeconds(damageDelay);
        }
        damageCoroutine = null;
    }

    void ApplyDamage(float damage)
    {
        target.DecreaseHP(damage);
    }
}
