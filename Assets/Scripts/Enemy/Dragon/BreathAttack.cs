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
        // isBurning�� true�� �Ǿ� ��ƼŬ ���� ���� ���� ���� -> ȭ�� ����� �ڷ�ƾ ����
        if (target.isBurning && !prevPlayerBurning)
        {
            // �ڷ�ƾ�� �ߺ� ȣ��Ǵ� ���� ����
            if (damageCoroutine == null)
                damageCoroutine = StartCoroutine(CoFireDamage());
        }
        // �̹� �����ӿ��� ��ƼŬ ������ Ż�� -> �ڷ�ƾ �ߴ�
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


    // delay �ʸ��� �� �� ������� �ִ� �ڷ�ƾ
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
