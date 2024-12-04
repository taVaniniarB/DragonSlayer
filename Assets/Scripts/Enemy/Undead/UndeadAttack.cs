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
    // �ִϸ��̼� �̺�Ʈ �Լ�
    public void AttackStart()
    {
        weaponCollider.enabled = true;
    }

    public void AttackEnd()
    {
        weaponCollider.enabled = false;
    }
}
