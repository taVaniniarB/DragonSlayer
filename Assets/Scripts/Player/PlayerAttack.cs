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
    public bool isAttackable = true; // ���� ���� Ÿ�̹� ����
    bool comboInput = false;
    int curComboCnt = 0;


    private CharacterController _controller;

    void Start()
    {
        TryGetComponent(out animator);
        audioSource = GetComponent<AudioSource>();
        _controller = GetComponent<CharacterController>();
        weaponCollider.enabled = false;
    }

    void Update()
    {
        HandleAttackInput();
    }

    void HandleAttackInput()
    {
        // ���� ���� Ÿ�ֿ̹� ��Ŭ�� �� ȣ��
        if (Input.GetMouseButtonDown(0) && isAttackable)
        {
            isAttackable = false;
            ++curComboCnt;
            comboInput = true;
            
            // ���� ù ������ ��
            if (curComboCnt == 1)
            {
                // �ִϸ��̼� Ʈ����
                animator.SetTrigger("Attack");

                // ù ������ �޺��׼��� �ƴϱ� ������ false�� ������ش�
                comboInput = false;
            }
        }
    }

    //
    // �÷��̾� ���� �ִϸ��̼� �̺�Ʈ �Լ�
    //

    public void PlayAttackSFX() // Į �ֵθ��� ȿ���� ���
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

    // ���� ���� Ÿ�̹� �̺�Ʈ
    public void ComboInputStart() // �Է� ���� ���������� ȣ��
    {
        isAttackable = true;
    }
    public void ComboInputEnd() // �Է� �׸� �޴� �������� ȣ��
    {
        isAttackable = false;

        // ���� �Է� ���� �ð� ���� �޺� �Է��� �����ٸ�, attack �ִϸ��̼��� exit�Ѵ�
        if (!comboInput)
        {
            ExitAttack();
        }

        // �Է� ���� �ʱ�ȭ
        comboInput = false;
    }

    // Attack ���� ���� �ʱ�ȭ
    public void ExitAttack()
    { 
        //Debug.Log("Exit Attack Called");
        isAttackable = true;
        curComboCnt = 0;
        WeaponCollisionDisable();
        animator.SetTrigger("ExitAttack");
    }
}
