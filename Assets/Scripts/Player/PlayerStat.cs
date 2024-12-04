using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public float maxHP = 100f;
    public float curHP;

    public float maxMP = 100f;
    public float curMP;

    public bool isBurning = false; // ȭ�� ��ƼŬ ���� ���� �ִ°�
    bool wasBurning = false; // ���� �����ӿ��� ȭ�� ��ƼŬ ���� ���� �־��°�

    public bool godMode = false; // ���� ���

    public float fireTime = 1f;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        curHP = maxHP;
        curMP = maxMP;
    }
    void Update()
    {
        if (!wasBurning)
        {
            isBurning = false;
        }
        wasBurning = false;
    }

    public void DecreaseHP(float amount)
    {
        if (godMode) return;

        curHP -= amount;
        CheckDeath();
    }
    public void IncreaseHP(float amount)
    {
        curHP += amount;
    }

    public void DecreaseMP(float amount)
    {
        curMP -= amount;
    }
    public void IncreaseMP(float amount)
    {
        curMP += amount;
    }

    void OnParticleCollision(GameObject other)
    {
        isBurning = true;
        wasBurning = true;
    }

    private void CheckDeath()
    {
        if (curHP <= 0)
        {
            PlayerDeath();
        }
    }
    private void PlayerDeath()
    {
        animator.SetTrigger("Die");
        UIManager.Instance.EnableDieMenu(true);
        Cursor.lockState = CursorLockMode.None;

        GetComponent<PlayerInputController>().Disable();

        GetComponent<PlayerMouseLook>().enabled = false;
    }

    IEnumerator Extinguish()
    {
        yield return new WaitForSeconds(fireTime);
        isBurning = false;
    }
}
