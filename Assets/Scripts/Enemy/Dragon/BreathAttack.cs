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
        // isBurning�� true�� �Ǿ� ��ƼŬ ���� ���� ���� �������� �� �ڷ�ƾ�� �����Ѵ�
        if (!prevPlayerBurning)
        {
            StartCoroutine(CoFireDamage());
        }
        prevPlayerBurning = target.isBurning;
    }


    // delay �ʸ��� �� �� ������� �ִ� �ڷ�ƾ
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
