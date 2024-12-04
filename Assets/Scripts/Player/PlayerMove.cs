using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float walkSpeed = 3.5f;
    public float runSpeed = 6.0f;
    public float turnSpeed = 6f;

    // 감속/가속 정도
    public float speedChangeRate = 10.0f;

    public float jumpHeight = 1.2f;
    public float gravity = -10f;

    // 연속점프 딜레이
    public float jumpDelay = 0.5f;

    // 공중에서 fall state로 전환되기까지의 시간. Useful for walking down stairs
    public float fallDelay = 0.15f;

    public bool isGround = true;

    // ground 계산 시 오차 범위
    public float groundCheckOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float groundCheckRadius = 0.28f;

    public LayerMask groundLayer;

    // player
    float speed;
    float _animationBlend;
    float targetRotation = 0f;
    float verVelocity;
    float maxVerVelocity = 53.0f;

    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    Animator animator;
    CharacterController controller;
    PlayerInputController playerInput;
    GameObject mainCam;

    private const float mouseThreshold = 1e-2f;

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInputController>();


        // reset our timeouts on start
        _jumpTimeoutDelta = jumpDelay;
        _fallTimeoutDelta = fallDelay;
    }


    void Update()
    {
        JumpAndFreeFall();
        GroundCheck();
        Move();
    }


    private void GroundCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundCheckOffset, transform.position.z);
        isGround = Physics.CheckSphere(spherePosition, groundCheckRadius, groundLayer);

        animator.SetBool("Grounded", isGround);
    }
    private void Move()
    {
        if (playerInput.move == Vector2.zero)
            speed = 0;
        else
            speed = runSpeed;

        _animationBlend = Mathf.Lerp(_animationBlend, speed, Time.deltaTime * speedChangeRate);
        
        if (_animationBlend < 1e-2f)
            _animationBlend = 0f;

        // wasd입력을 xz평면의 유닛벡터로 변환
        Vector3 moveInputDir = new Vector3(playerInput.move.x, 0.0f, playerInput.move.y).normalized;

        // 이동 입력 존재 시 rotation 설정
        if (playerInput.move != Vector2.zero)
        {
            // TransformDirection : 해당 transform의 로컬공간에서의 벡터를 월드공간에서의 벡터로 바꿔주는 함수
            Vector3 worldInputDir = mainCam.transform.TransformDirection(moveInputDir); // 카메라 기준 월드 좌표로 변환
            worldInputDir.y = 0.0f;
            worldInputDir.Normalize();

            Quaternion lookRotation = Quaternion.LookRotation(worldInputDir);
            
            targetRotation = lookRotation.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
        }

        Vector3 moveDir = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        // 플레이어 이동
        controller.Move(moveDir.normalized * (speed * Time.deltaTime) +
                         new Vector3(0.0f, verVelocity, 0.0f) * Time.deltaTime);

        animator.SetFloat("Speed", _animationBlend);
        animator.SetFloat("MotionSpeed", 1f);
    }

    private void JumpAndFreeFall()
    {
        if (isGround)
        {
            _fallTimeoutDelta = fallDelay;

            animator.SetBool("Jump", false);
            animator.SetBool("FreeFall", false);

            if (verVelocity < 0.0f)
            {
                verVelocity = -2f;
            }

            // Jump
            if (playerInput.jump && _jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                verVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

                animator.SetBool("Jump", true);
            }

            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else  // 그라운드 상태가 아닐 때 (자유낙하)
        {
            // 이전에 자유낙하 시간이 경과했다
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else // 자유낙하 상태에 진입
            { 
                animator.SetBool("FreeFall", true);
            }

            // 점프 변수들 초기화
            _jumpTimeoutDelta = jumpDelay;
            playerInput.jump = false;
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (verVelocity < maxVerVelocity)
        {
            verVelocity += gravity * Time.deltaTime;
        }
    }
}
