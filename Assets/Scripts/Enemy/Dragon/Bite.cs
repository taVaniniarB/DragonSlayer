using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// jaw collider에 붙는 스크립트
public class Bite : MonoBehaviour
{
    PlayerState target;
    float damage;

    void Start()
    {
        target = GameManager.Instance.playerInst.GetComponent<PlayerState>();
        damage = transform.root.GetComponent<BiteAttack>().biteDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("물기 공격에 피격");
            target.DecreaseHP(damage);
        }
    }
}
