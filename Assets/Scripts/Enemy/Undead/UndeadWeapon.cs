using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadWeapon : MonoBehaviour
{
    PlayerState target;
    public Undead undead;

    void Start()
    {
        target = GameManager.Instance.playerInst.GetComponent<PlayerState>();
    }
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Attacked by Undead's Weapon");
            target.DecreaseHP(undead.ATK);
        }
    }
}
