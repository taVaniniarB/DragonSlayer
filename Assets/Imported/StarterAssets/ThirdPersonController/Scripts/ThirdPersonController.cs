using UnityEngine;
using UnityEngine.InputSystem;

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {

        [Header("Player")]
        public float MoveSpeed = 2.0f;
        public float sprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        public float JumpHeight = 1.2f;
        public float Gravity = -15f;

        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.5f;

        //fall state로 전환되기까지의 시간. Useful for walking down stairs
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        //시네머신이 촬영할 대상
        public GameObject cinemachineTarget;

        public float TopClamp = 70.0f;
        public float BottomClamp = -30.0f;

        public float sensabilityYaw = 2.0f;
        public float sensabilityPitch = 1.5f;

        // 모든 축에서의 카메라 위치 잠금
        public bool cameraLock = false;

        private float cinemachineTargetYaw;
        private float cinemachineTargetPitch;

        // player
        private float speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

        private Animator animator;
        private CharacterController controller;
        private StarterAssetsInputs playerInput;
        private GameObject mainCam;

        private const float mouseThreshold = 1e-2f;

        private bool hasAnimator;


        private void Awake()
        {
            // get a reference to our main camera
            if (mainCam == null)
            {
                mainCam = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            cinemachineTargetYaw = cinemachineTarget.transform.rotation.eulerAngles.y;

            hasAnimator = TryGetComponent(out animator);
            controller = GetComponent<CharacterController>();
            playerInput = GetComponent<StarterAssetsInputs>();

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            hasAnimator = TryGetComponent(out animator);

            JumpAndGravity();
            GroundedCheck();
            Move();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            if (hasAnimator)
            {
                animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {

            // target이 회전하면 시네머신 카메라는 target을 따라 회전하므로
            // 마우스 이동량 * 감도 만큼 target을 회전시켜준다


            // 마우스 이동량이 최소 임계값를 넘었을 때((매우 작은 움직임 방지)
            if ((playerInput.look.sqrMagnitude >= mouseThreshold) && !cameraLock)
            {
                cinemachineTargetYaw += playerInput.look.x * sensabilityYaw;
                cinemachineTargetPitch += playerInput.look.y * sensabilityPitch;
            }

            cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);

            cinemachineTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch,
                cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            float targetSpeed = sprintSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (playerInput.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float curHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

            float speedOffset = 0.1f;

            // accelerate or decelerate to target speed
            if (curHorizontalSpeed < targetSpeed - speedOffset ||
                curHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                speed = Mathf.Lerp(curHorizontalSpeed, targetSpeed,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                speed = Mathf.Round(speed * 1000f) / 1000f;
            }
            else
            {
                speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // wasd입력을 xz평면의 유닛벡터로 변환
            Vector3 inputDir = new Vector3(playerInput.move.x, 0.0f, playerInput.move.y).normalized;
            if (playerInput.move.magnitude != 0) // 이동 입력이 존재할 때
            {

            }
            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (playerInput.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg +
                                  mainCam.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDir = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // 플레이어 이동
            controller.Move(targetDir.normalized * (speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (hasAnimator)
            {
                animator.SetFloat(_animIDSpeed, _animationBlend);
                animator.SetFloat(_animIDMotionSpeed, 1);
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (hasAnimator)
                {
                    animator.SetBool(_animIDJump, false);
                    animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (playerInput.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (hasAnimator)
                    {
                        animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (hasAnimator)
                    {
                        animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                playerInput.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
        /*
                private void OnDrawGizmosSelected()
                {
                    Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
                    Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

                    if (Grounded) Gizmos.color = transparentGreen;
                    else Gizmos.color = transparentRed;

                    // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
                    Gizmos.DrawSphere(
                        new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                        GroundedRadius);
                }*/

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(controller.center), FootstepAudioVolume);
            }
        }
    }
}