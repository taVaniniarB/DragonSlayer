using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHP = 100f;
    public float curHP;

    void Start()
    {
        curHP = maxHP;
    }

    public void DecreaseHP(float amount)
    {
        curHP -= amount;
        CheckDeath();
    }
    void CheckDeath()
    {
        Animator animator = GetComponent<Animator>();

        if (animator.GetBool("Die")) return;

        if (curHP <= 0)
        {
            animator.SetBool("Die", true);

            GameManager gm = GameManager.Instance;
            if (gm.currentState == GameState.Exploration)
                gm.RemoveMonsterFromList(GetComponent<Undead>());
            else if (gm.currentState == GameState.BossBattle)
                gm.BossDefeated();
        }


    }
    public void IncreaseHP(float amount)
    {
        curHP += amount;
    }
}
