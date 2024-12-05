using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Animator animator;
    public BoxCollider weaponCollider;
    [Header("Swing Sound")]
    public AudioClip[] SwingAudioClips;
    public float SwingVolume = 1f;
    private AudioSource audioSource;
    [Header("Combo Setting")]
    public bool isAttackable = true; // 공격 가능 타이밍 제어
    public bool comboInput = false;
    public int curComboCnt = 0;


    void Start()
    {
        TryGetComponent(out animator);
        audioSource = GetComponent<AudioSource>();
        weaponCollider.enabled = false;
    }

    void Update()
    {
        HandleAttackInput();
    }

    void HandleAttackInput()
    {
        // 공격 가능 타이밍에 좌클릭 시 호출
        if (Input.GetMouseButtonDown(0) && isAttackable)
        {
            isAttackable = false;
            ++curComboCnt;
            comboInput = true;
            
            // 공격 첫 실행일 때
            if (curComboCnt == 1)
            {
                // 애니메이션 트리거
                animator.SetTrigger("Attack");

                // 첫 실행은 콤보액션이 아니기 때문에 false로 만들어준다
                comboInput = false;
            }
        }
    }

    //
    // 플레이어 공격 애니메이션 이벤트 함수
    //

    public void PlayAttackSFX() // 칼 휘두르는 효과음 재생
    {
        var index = Random.Range(0, SwingAudioClips.Length);
        audioSource.PlayOneShot(SwingAudioClips[index], SwingVolume);
    }

    public void WeaponCollisionEnable()
    {
        weaponCollider.enabled = true;
    }

    public void WeaponCollisionDisable()
    {
        weaponCollider.enabled = false;
    }

    // 공격 가능 타이밍 이벤트
    public void ComboInputStart() // 입력 받을 시작점에서 호출
    {
        isAttackable = true;
    }
    public void ComboInputEnd() // 입력 그만 받는 시점에서 호출
    {
        isAttackable = false;

        // 이전 입력 가능 시간 동안 콤보 입력이 없었다면, attack 애니메이션을 exit한다
        if (!comboInput)
        {
            ExitAttack();
        }

        // 입력 정보 초기화
        comboInput = false;
    }

    // 콤보 관련 변수 초기화
    public void ExitAttack()
    {
        InitAttackCombo();
        animator.SetTrigger("ExitAttack");
    }

    public void InitAttackCombo()
    {
        isAttackable = true;
        curComboCnt = 0;
        WeaponCollisionDisable();
        animator.ResetTrigger("ExitAttack");
    }
}
