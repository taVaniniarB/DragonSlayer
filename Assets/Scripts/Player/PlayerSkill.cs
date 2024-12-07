using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    Animator animator;
    public GameObject slashPrefab;
    AudioSource audioSource;
    public AudioClip skillSFX;
    public float skillSFXVolume = 1f;
    public Vector3 spawnOffset = new Vector3(0f, 1.3f, 1.2f);
    public float skillCost = 10f;
    public float skillDelayTime = 5f;
    public float slashSpeed = 20f;
    public float slashLifeTime = 3f;
    float curElapsed; // ��ų ��� �� ��� �ð�
    public bool canSkill = true;
    PlayerStat state;
    private Camera mainCamera;
    void Start()
    {
        state = GetComponent<PlayerStat>();
        animator = GetComponent<Animator>();
        curElapsed = skillDelayTime;
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        CanUseSkill();
        SetSkillUI();
        HandleSkillInput();
    }
    void CanUseSkill()
    {
        curElapsed += Time.deltaTime;

        // ���� ���Ұ� MP�� ����� ��
        if (curElapsed >= skillDelayTime && state.curMP >= skillCost)
            canSkill = true;
    }
    void SetSkillUI()
    {
        // ��� �ð��� ���� UI�� ����� ����
        if (canSkill)
        {

        }
    }
    void HandleSkillInput()
    {
        if (Input.GetMouseButtonDown(1) && canSkill)
        {
            // ��ų ����
            animator.SetTrigger("Skill");

            // ��Ÿ��, ��ų ��� ���� ���� �ʱ�ȭ
            curElapsed = 0f;
            canSkill = false;
        }
    }

    public void Shoot()
    {
        Vector3 spawnPos = transform.position
                                + transform.forward * spawnOffset.z
                                + Vector3.up * spawnOffset.y;

        // ����ü ���� ���� (ī�޶� ���߾��� ���� ���ư�����)
        Vector3 fireDir = mainCamera.transform.forward;

        GameObject slash = Instantiate(slashPrefab, spawnPos, Quaternion.LookRotation(fireDir));

        slash.GetComponent<Rigidbody>().velocity = fireDir * slashSpeed;

        Destroy(slash, slashLifeTime);


        // ȿ���� ���
        //SkillSFX.PlayOneShot(SwingAudioClips[index], SwingVolume);
        audioSource.PlayOneShot(skillSFX, skillSFXVolume);
        state.DecreaseMP(skillCost);

    }
}
