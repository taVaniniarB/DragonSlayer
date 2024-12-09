using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float walkSpeed = 3.5f;
    public float runSpeed = 6.0f;
    public float turnSpeed = 6f;

    // �ִϸ��̼� ���� �ӵ�
    public float speedChangeRate = 10.0f;

    public float jumpHeight = 1.2f;
    public float gravity = -10f;

    // �������� ������
    public float jumpDelay = 0.5f;

    // ���߿��� fall state�� ��ȯ�Ǳ������ �ð�
    public float fallDelay = 0.15f;

    public bool isGround = true;

    // ground ��� �� ���� ����
    public float groundCheckOffset = -0.14f;
    public float groundCheckRadius = 0.28f;

    public LayerMask groundLayer;

    // player
    float speed;
    float animationBlend;
    float targetRotation = 0f;
    float verVelocity;
    float maxVerVelocity = 53.0f;

    // Ÿ�̸�
    private float curJumpTime;
    private float curFallTime;

    Animator animator;
    CharacterController controller;
    PlayerInputController playerInput;
    GameObject mainCam;

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInputController>();
        animator.SetFloat("MotionSpeed", 1f);
        curJumpTime = jumpDelay;
        curFallTime = fallDelay;
    }


    void Update()
    {
        JumpAndFreeFall();
        CheckGround();
        Move();
    }


    private void CheckGround()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundCheckOffset, transform.position.z);
        isGround = Physics.CheckSphere(spherePosition, groundCheckRadius, groundLayer);

        animator.SetBool("Grounded", isGround);
    }
    private void Move()
    {
        speed = (playerInput.move == Vector2.zero) ? 0 : runSpeed;

        animationBlend = Mathf.Lerp(animationBlend, speed, Time.deltaTime * speedChangeRate);
        
        if (animationBlend < 1e-2f)
            animationBlend = 0f;

        Vector3 moveInputDir = new Vector3(playerInput.move.x, 0.0f, playerInput.move.y).normalized;

        // �̵� �Է� ���� �� rotation ����
        if (playerInput.move != Vector2.zero)
        {
            // ī�޶� ���� �̵� ����
            // TransformDirection : �ش� transform�� ���ð��������� ���͸� ������������� ���ͷ� �ٲ��ִ� �Լ�
            Vector3 worldInputDir = mainCam.transform.TransformDirection(moveInputDir);
            worldInputDir.y = 0.0f;
            worldInputDir.Normalize();

            Quaternion lookRotation = Quaternion.LookRotation(worldInputDir);
            
            targetRotation = lookRotation.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
        }

        // �̵��� ����
        Vector3 moveDir = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        // �÷��̾� �̵�
        controller.Move(moveDir.normalized * (speed * Time.deltaTime) +
                         new Vector3(0.0f, verVelocity, 0.0f) * Time.deltaTime);

        animator.SetFloat("Speed", animationBlend);
    }

    private void JumpAndFreeFall()
    {
        if (isGround)
        {
            curFallTime = fallDelay;

            animator.SetBool("Jump", false);
            animator.SetBool("FreeFall", false);

            if (verVelocity < 0.0f)
            {
                verVelocity = -2f;
            }

            // Jump
            if (playerInput.jump && curJumpTime <= 0.0f)
            {
                // jumpHeight�� �����ϱ� ���� �ʿ��� �ʱ� �ӵ�
                verVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

                animator.SetBool("Jump", true);
            }

            if (curJumpTime >= 0.0f)
            {
                curJumpTime -= Time.deltaTime;
            }
        }
        else  // Ground ���°� �ƴ� �� (��������)
        {
            // ������ �������� �ð��� ����ߴ�
            if (curFallTime >= 0.0f)
            {
                curFallTime -= Time.deltaTime;
            }
            else // �������� ���¿� ����
            { 
                animator.SetBool("FreeFall", true);
            }

            // ���� ������ �ʱ�ȭ
            curJumpTime = jumpDelay;
            playerInput.jump = false;
        }

        // �ִ� �ӵ� �̸��� �� �߷� ���ӵ� �ο�
        if (verVelocity < maxVerVelocity)
        {
            verVelocity += gravity * Time.deltaTime;
        }
    }
}
