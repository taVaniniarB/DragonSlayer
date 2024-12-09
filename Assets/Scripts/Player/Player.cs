using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float ATK = 25f;
    public float SkillDamage;
    Animator animator;
    PlayerState stat;

    void Awake()
    {
        if (GameManager.Instance)
            GameManager.Instance.RegisterPlayer(this);
    }
    void Start()
    {
        GameManager.Instance.StartGame();
        animator = GetComponent<Animator>();
        stat = GetComponent<PlayerState>();
        SkillDamage = ATK * 2;
    }

    void Update()
    {
        // 회피 트리거
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetTrigger("Dodge");
            stat.DecreaseMP(3f);
        }


        // HP 즉시회복 치트키 (C)
        if (Input.GetKeyDown(KeyCode.C))
        {
            stat.IncreaseHP(100);
        }
        // MP 즉시회복 치트키 (V)
        if (Input.GetKeyDown(KeyCode.V))
        {
            stat.IncreaseMP(100);
        }
    }
}
