using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BiteAttack : MonoBehaviour
{
    public float AttackTime = 0.2f;
    public bool isAttacking = false;
    public BoxCollider jawCollider;
    public float biteDamage = 10;


    public void Attack()
    {
        StartCoroutine(CoAttack());
    }
    
    
    IEnumerator CoAttack()
    {
        isAttacking = true;
        jawCollider.enabled = true;
        yield return new WaitForSeconds(AttackTime);
        isAttacking = false;
        jawCollider.enabled = false;
    }
}
