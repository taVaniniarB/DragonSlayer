using Kalagaan;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Dragon : MonoBehaviour
{
    [Header("Speed")]
    public float walkSpeed = 1.5f;
    public float GroundChargeSpeed = 12f;
    public float FlySpeed = 10f;
    public float takeOffSpeed = 2f;

    [Header("Charge")]
    public float groundChargeTime = 4f;
    public float flyChargeTime = 5f;

    [Header("Mode")]
    public float flyModeTime = 15f;
    public float groundModeTime = 15f;
    public bool isGround = true;
    float curGroundTime = 0;

    float takeOffTime = 0.5f;
    float curTakeOffTime = 0f;

    public Transform jaw;

    public float jawOpenRatio = 0f;
    public Vector2 jawLimit = new Vector2(-3f, 70f);

    Animator animator;

    public bool isBreathing = false;
    public FireVFX fire;

    public Transform target;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // ground �ð��� ä�� �� fly�� ��ȯ, ground ������ �ʱ�ȭ
        if ((curGroundTime >= groundModeTime) && (animator.GetBool("Walk") || animator.GetBool("Run")))
        {
            EnterFly();
        }

        UpdateGround();
    }

    void LateUpdate()
    {
        UpdateJaw();

        UpdateYPos();
    }

    private void UpdateJaw()
    {
        jawOpenRatio = Mathf.Clamp01(jawOpenRatio);
        if (jaw != null)
        {
            if (jawOpenRatio > 0)
                jaw.localRotation = Quaternion.Euler(0f, Mathf.Lerp(jawLimit.x, jawLimit.y, jawOpenRatio), 0f); // mark
        }
    }

    private void UpdateYPos()
    {
        // dragon �Ʒ��� ray ���� y�� ���� (�� ���� ����)
        if (isGround)
        {
            RaycastHit hitDown, hitUp;
            int layerMask = LayerMask.GetMask("Ground");
            float newY = transform.position.y;

            // �Ʒ��� Raycast
            bool hitGroundBelow = Physics.Raycast(transform.position, Vector3.down, out hitDown, Mathf.Infinity, layerMask);

            // ���� Raycast
            Vector3 high = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
            bool hitGroundAbove = Physics.Raycast(high, Vector3.down, out hitUp, Mathf.Infinity, layerMask);

            if (hitGroundBelow && transform.position.y > hitDown.point.y)
            {
                // �巡���� ������ ���� �ִ� ���
                newY = hitDown.point.y;
            }
            else if (hitGroundAbove && transform.position.y < hitUp.point.y)
            {
                // �巡���� ������ �Ʒ��� �ִ� ���
                newY = hitUp.point.y;
            }

            // ��ġ ����
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    public void BreathStart()
    {
        jawOpenRatio = 1f;
        fire.BreathStart();
    }

    public void BreathEnd()
    {
        jawOpenRatio = 0f;
        fire.BreathEnd();
    }

    private void UpdateGround()
    {
        if (isGround)
        {
            curGroundTime += Time.deltaTime;
            float jawPlayerDist = Vector3.Distance(target.transform.position, jaw.position);
            float dragonPlayerDist = Vector3.Distance(target.transform.position, transform.position);

            animator.SetFloat("JawPlayerDist", jawPlayerDist);
            animator.SetFloat("DragonPlayerDist", dragonPlayerDist);
        }
    }

    private void EnterFly()
    {
        animator.SetBool("Fly", true);
        animator.SetBool("Walk", false);
        curGroundTime = 0f;
    }

    // Land �ִϸ��̼ǿ��� ���� ���� �̺�Ʈ�� ȣ��
    public void Land()
    {
        isGround = true;
    }

    // take_off �ִϸ��̼ǿ��� �̷� ���� �̺�Ʈ�� ȣ��
    public void TakeOff()
    {
        StartCoroutine(CoTakeOff());
    }

    IEnumerator CoTakeOff()
    {
        float startY = transform.position.y;
        float targetHeight = startY + FlySpeed * takeOffTime; // ��ǥ ���� ���

        while (curTakeOffTime < takeOffTime)
        {
            float newY = Mathf.Lerp(startY, targetHeight, curTakeOffTime / takeOffTime);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            curTakeOffTime += Time.deltaTime;

            yield return null;
        }

        curTakeOffTime = 0;
        Debug.Log("Takeoff complete");
    }
}
