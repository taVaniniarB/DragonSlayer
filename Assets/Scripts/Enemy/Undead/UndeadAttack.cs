using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadAttack : MonoBehaviour
{
    public BoxCollider weaponCollider;

    void Start()
    {
        weaponCollider.enabled = false;
    }

    //
    // 애니메이션 이벤트 함수
    public void AttackStart()
    {
        weaponCollider.enabled = true;
    }

    public void AttackEnd()
    {
        weaponCollider.enabled = false;
    }
}
