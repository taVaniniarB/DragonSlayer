using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonLookAt : MonoBehaviour
{
    [Header("Transforms")]
    public Transform headBone;    // �Ӹ� ��
    public Transform target;      // �ٶ� ��� (�÷��̾�)

    [Header("Rotation Settings")]
    public float rotationSpeed = 5f;  // �Ӹ� ȸ�� �ӵ�
    public float maxYawAngle = 60f;   // �¿� ȸ�� ����
    public float maxPitchAngle = 30f; // ���Ʒ� ȸ�� ����

    private Animator animator;        // �ִϸ�����

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void LastUpaate()
    {
        if (target == null || headBone == null) return;

        // 1. ��ǥ ���� ���
        Vector3 targetDirection = (target.position - headBone.position).normalized;

        // 2. ���� �������� ��ȯ
        Transform rootBone = headBone.parent; // �Ӹ� ���� �θ�(��Ʈ ��)
        Vector3 localDirection = rootBone.InverseTransformDirection(targetDirection);

        // 3. ���� ���� ����
        Vector2 clampedAngles = ClampDirectionToLimits(localDirection);

        // 4. ��ǥ ȸ�� ���
        Quaternion targetRotation = Quaternion.Euler(clampedAngles.y, clampedAngles.x, 0f);

        // 5. �Ӹ� �� ȸ�� ����
        headBone.localRotation = Quaternion.Slerp(
            headBone.localRotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    /// <summary>
    /// ���� ���� ���͸� ������� ������ �����մϴ�.
    /// </summary>
    /// <param name="localDir">��Ʈ �� ���� ���������� ���� ����</param>
    /// <returns>Yaw(Pitch), Pitch(Yaw)</returns>
    private Vector2 ClampDirectionToLimits(Vector3 localDir)
    {
        // Yaw (�¿� ���� ����)
        float yaw = Mathf.Atan2(localDir.x, localDir.z) * Mathf.Rad2Deg;
        yaw = Mathf.Clamp(yaw, -maxYawAngle, maxYawAngle);

        // Pitch (���Ʒ� ���� ����)
        float pitch = Mathf.Asin(localDir.y) * Mathf.Rad2Deg;
        pitch = Mathf.Clamp(pitch, -maxPitchAngle, maxPitchAngle);

        return new Vector2(yaw, pitch);
    }
}
