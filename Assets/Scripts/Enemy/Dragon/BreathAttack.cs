using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathAttack : MonoBehaviour
{
    public PlayerStat target;
    public float damagePerTick = 2f;
    public float damageDelay = 0.2f;
    bool prevPlayerBurning = false;

    void Update()
    {
        // isBurning이 true가 되어 파티클 범위 내에 진입 시작했을 때 코루틴을 시작한다
        if (!prevPlayerBurning)
        {
            StartCoroutine(CoFireDamage());
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
    }

    void ApplyDamage(float damage)
    {
        target.DecreaseHP(damage);
    }
}
