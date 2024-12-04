using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float ATK = 25f;
    public float SkillDamage;
    Animator animator;
    PlayerStat PlayerHealth;

    void Awake()
    {
        if (GameManager.Instance)
            GameManager.Instance.RegisterPlayer(this);
    }
    void Start()
    {
        GameManager.Instance.StartGame();
        animator = GetComponent<Animator>();
        PlayerHealth = GetComponent<PlayerStat>();
        SkillDamage = ATK * 2;
    }

    void Update()
    {
        // 회피 트리거
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetTrigger("Dodge");
            PlayerHealth.DecreaseMP(3f);
        }
    }
}
