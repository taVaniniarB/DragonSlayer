using Kalagaan;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    [Header("Speed")]
    public float walkSpeed = 1.5f;
    public float GroundChargeSpeed = 12f;
    public float FlySpeed = 10f;
    public float takeOffSpeed = 2f;

    [Header("Charge")]
    public float groundChargeTime = 5f;
    public float flyChargeTime = 5f;

    [Header("Mode")]
    public float flyModeTime = 15f;
    public float groundModeTime = 15f;
    public bool isGround = true;
    float curGroundTime = 0;

    public float flyAltitude = 40f;
    float takeOffTime = 0.5f;
    float curTakeOffTime = 0f;

    public BoxCollider[] foots;
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

        UpdateGroundVars();
    }

    void LateUpdate()
    {
        //jaw
        jawOpenRatio = Mathf.Clamp01(jawOpenRatio);
        if (jaw != null)
        {
            if (jawOpenRatio > 0)
                jaw.localRotation = Quaternion.Euler(-180f, Mathf.Lerp(jawLimit.x, jawLimit.y, jawOpenRatio), 0f);
        }
    }
    public void BreathStart()
    {
        // ���� �� ���� ���� ��ƼŬ On
        jawOpenRatio = 1f;
        fire.BreathStart();
    }

    public void BreathEnd()
    {
        jawOpenRatio = 0f;
        fire.BreathEnd();
    }

    private void UpdateGroundVars()
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
        //isGround = false; <- ��¥ fly ���� �������� ����ǵ��� ����
        curGroundTime = 0f;
    }

    // Land �ִϸ��̼ǿ��� ���� ���� �̺�Ʈ�� ȣ��
    public void Land()
    {
        isGround = true;
    }

    // take_off �ִϸ��̼ǿ��� �̷� ���� �̺�Ʈ�� ȣ��
    public void takeOff()
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
