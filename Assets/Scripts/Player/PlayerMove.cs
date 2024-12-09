using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float walkSpeed = 3.5f;
    public float runSpeed = 6.0f;
    public float turnSpeed = 6f;

    // 애니메이션 보간 속도
    public float speedChangeRate = 10.0f;

    public float jumpHeight = 1.2f;
    public float gravity = -10f;

    // 연속점프 딜레이
    public float jumpDelay = 0.5f;

    // 공중에서 fall state로 전환되기까지의 시간
    public float fallDelay = 0.15f;

    public bool isGround = true;

    // ground 계산 시 오차 범위
    public float groundCheckOffset = -0.14f;
    public float groundCheckRadius = 0.28f;

    public LayerMask groundLayer;

    // player
    float speed;
    float animationBlend;
    float targetRotation = 0f;
    float verVelocity;
    float maxVerVelocity = 53.0f;

    // 타이머
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

        // 이동 입력 존재 시 rotation 설정
        if (playerInput.move != Vector2.zero)
        {
            // 카메라 기준 이동 방향
            // TransformDirection : 해당 transform의 로컬공간에서의 벡터를 월드공간에서의 벡터로 바꿔주는 함수
            Vector3 worldInputDir = mainCam.transform.TransformDirection(moveInputDir);
            worldInputDir.y = 0.0f;
            worldInputDir.Normalize();

            Quaternion lookRotation = Quaternion.LookRotation(worldInputDir);
            
            targetRotation = lookRotation.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
        }

        // 이동할 방향
        Vector3 moveDir = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        // 플레이어 이동
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
                // jumpHeight에 도달하기 위해 필요한 초기 속도
                verVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

                animator.SetBool("Jump", true);
            }

            if (curJumpTime >= 0.0f)
            {
                curJumpTime -= Time.deltaTime;
            }
        }
        else  // Ground 상태가 아닐 때 (자유낙하)
        {
            // 이전에 자유낙하 시간이 경과했다
            if (curFallTime >= 0.0f)
            {
                curFallTime -= Time.deltaTime;
            }
            else // 자유낙하 상태에 진입
            { 
                animator.SetBool("FreeFall", true);
            }

            // 점프 변수들 초기화
            curJumpTime = jumpDelay;
            playerInput.jump = false;
        }

        // 최대 속도 미만일 때 중력 가속도 부여
        if (verVelocity < maxVerVelocity)
        {
            verVelocity += gravity * Time.deltaTime;
        }
    }
}
