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
        // ȸ�� Ʈ����
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetTrigger("Dodge");
            stat.DecreaseMP(3f);
        }


        // HP ���ȸ�� ġƮŰ (C)
        if (Input.GetKeyDown(KeyCode.C))
        {
            stat.IncreaseHP(100);
        }
        // MP ���ȸ�� ġƮŰ (V)
        if (Input.GetKeyDown(KeyCode.V))
        {
            stat.IncreaseMP(100);
        }
    }
}
