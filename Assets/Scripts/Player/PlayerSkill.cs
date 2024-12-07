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
    float curElapsed; // 스킬 사용 후 경과 시간
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

        // 쿨이 돌았고 MP가 충분할 때
        if (curElapsed >= skillDelayTime && state.curMP >= skillCost)
            canSkill = true;
    }
    void SetSkillUI()
    {
        // 경과 시간에 따라 UI의 모습을 변경
        if (canSkill)
        {

        }
    }
    void HandleSkillInput()
    {
        if (Input.GetMouseButtonDown(1) && canSkill)
        {
            // 스킬 실행
            animator.SetTrigger("Skill");

            // 쿨타임, 스킬 사용 가능 여부 초기화
            curElapsed = 0f;
            canSkill = false;
        }
    }

    public void Shoot()
    {
        Vector3 spawnPos = transform.position
                                + transform.forward * spawnOffset.z
                                + Vector3.up * spawnOffset.y;

        // 투사체 방향 설정 (카메라 정중앙을 향해 날아가도록)
        Vector3 fireDir = mainCamera.transform.forward;

        GameObject slash = Instantiate(slashPrefab, spawnPos, Quaternion.LookRotation(fireDir));

        slash.GetComponent<Rigidbody>().velocity = fireDir * slashSpeed;

        Destroy(slash, slashLifeTime);


        // 효과음 재생
        //SkillSFX.PlayOneShot(SwingAudioClips[index], SwingVolume);
        audioSource.PlayOneShot(skillSFX, skillSFXVolume);
        state.DecreaseMP(skillCost);

    }
}
