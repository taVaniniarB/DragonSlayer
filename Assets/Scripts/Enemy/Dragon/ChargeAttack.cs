using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : MonoBehaviour
{
    public bool isCharging = false;
    public float damage = 2;
    PlayerStat target;

    void Start()
    {
        target = GameManager.Instance.playerInst.GetComponent<PlayerStat>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isCharging)
        {
            Debug.Log("���� ���ݿ� �ǰ�");
            target.DecreaseHP(damage);
        }
    }
}
