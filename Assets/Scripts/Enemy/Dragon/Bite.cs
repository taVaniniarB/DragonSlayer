using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// jaw collider�� �ٴ� ��ũ��Ʈ
public class Bite : MonoBehaviour
{
    PlayerStat target;
    float damage;

    void Start()
    {
        target = GameManager.Instance.playerInst.GetComponent<PlayerStat>();
        damage = transform.root.GetComponent<BiteAttack>().biteDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("���� ���ݿ� �ǰ�");
            target.DecreaseHP(damage);
        }
    }
}
