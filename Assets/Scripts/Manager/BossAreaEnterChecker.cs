using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAreaEnterChecker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.bossAreaEntered = true;
        }
    }
}
