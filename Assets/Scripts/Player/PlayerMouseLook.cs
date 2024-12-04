using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseLook : MonoBehaviour
{
    //�ó׸ӽ��� �Կ��� ���
    public GameObject camTarget;

    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;

    public float sensabilityX = 2.0f;
    public float sensabilityY = 1.5f;

    // ��� �࿡���� ī�޶� ��ġ ���
    public bool cameraRotateLock = false;

    private float camRotationX;
    private float camRotationY;

    private PlayerInputController playerInput;

    private void Start()
    {
        camRotationX = camTarget.transform.rotation.eulerAngles.y;
        playerInput = GetComponent<PlayerInputController>();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        // target�� ȸ���ϸ� �ó׸ӽ� ī�޶�� target�� ���� ȸ���ϹǷ�
        // ���콺 �̵��� * ���� ��ŭ target�� ȸ�������ش�

        if (cameraRotateLock)
            return;

        camRotationX += playerInput.look.x * sensabilityX;
        camRotationY += playerInput.look.y * sensabilityY;
        
        camRotationY = Mathf.Clamp(camRotationY, BottomClamp, TopClamp);

        // �������� target�� ȸ��
        camTarget.transform.rotation = Quaternion.Euler(camRotationY, camRotationX, 0f);
    }
}
