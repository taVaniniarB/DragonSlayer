using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseLook : MonoBehaviour
{
    //시네머신이 촬영할 대상
    public GameObject camTarget;

    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;

    public float sensabilityX = 2.0f;
    public float sensabilityY = 1.5f;

    // 모든 축에서의 카메라 위치 잠금
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
        // target이 회전하면 시네머신 카메라는 target을 따라 회전하므로
        // 마우스 이동량 * 감도 만큼 target을 회전시켜준다

        if (cameraRotateLock)
            return;

        camRotationX += playerInput.look.x * sensabilityX;
        camRotationY += playerInput.look.y * sensabilityY;
        
        camRotationY = Mathf.Clamp(camRotationY, BottomClamp, TopClamp);

        // 최종적인 target의 회전
        camTarget.transform.rotation = Quaternion.Euler(camRotationY, camRotationX, 0f);
    }
}
