using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : MonoBehaviour
{
    public bool isCharging = false;
    public float damage = 2;
    PlayerState target;

    void Start()
    {
        target = GameManager.Instance.playerInst.GetComponent<PlayerState>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isCharging)
        {
            target.DecreaseHP(damage);
        }
    }
}
