using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonLookAt : MonoBehaviour
{
    [Header("Transforms")]
    public Transform headBone;    // 머리 본
    public Transform target;      // 바라볼 대상 (플레이어)

    [Header("Rotation Settings")]
    public float rotationSpeed = 5f;  // 머리 회전 속도
    public float maxYawAngle = 60f;   // 좌우 회전 제한
    public float maxPitchAngle = 30f; // 위아래 회전 제한

    private Animator animator;        // 애니메이터

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void LastUpaate()
    {
        if (target == null || headBone == null) return;

        // 1. 목표 방향 계산
        Vector3 targetDirection = (target.position - headBone.position).normalized;

        // 2. 로컬 방향으로 변환
        Transform rootBone = headBone.parent; // 머리 본의 부모(루트 본)
        Vector3 localDirection = rootBone.InverseTransformDirection(targetDirection);

        // 3. 각도 제한 적용
        Vector2 clampedAngles = ClampDirectionToLimits(localDirection);

        // 4. 목표 회전 계산
        Quaternion targetRotation = Quaternion.Euler(clampedAngles.y, clampedAngles.x, 0f);

        // 5. 머리 본 회전 적용
        headBone.localRotation = Quaternion.Slerp(
            headBone.localRotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    /// <summary>
    /// 로컬 방향 벡터를 기반으로 각도를 제한합니다.
    /// </summary>
    /// <param name="localDir">루트 본 로컬 공간에서의 방향 벡터</param>
    /// <returns>Yaw(Pitch), Pitch(Yaw)</returns>
    private Vector2 ClampDirectionToLimits(Vector3 localDir)
    {
        // Yaw (좌우 각도 제한)
        float yaw = Mathf.Atan2(localDir.x, localDir.z) * Mathf.Rad2Deg;
        yaw = Mathf.Clamp(yaw, -maxYawAngle, maxYawAngle);

        // Pitch (위아래 각도 제한)
        float pitch = Mathf.Asin(localDir.y) * Mathf.Rad2Deg;
        pitch = Mathf.Clamp(pitch, -maxPitchAngle, maxPitchAngle);

        return new Vector2(yaw, pitch);
    }
}
