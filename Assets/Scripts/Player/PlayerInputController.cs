using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool dodge;
    public bool sprint = true;

    bool enable = true;

    public void Disable()
    {
        enable = false;
    }

    public void OnMove(InputValue value)
    {
        if(enable)
            move = value.Get<Vector2>();
    }

    // 마우스 움직일 때 호출
    public void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (enable)
            jump = value.isPressed;
    }

    public void OnDodge(InputValue value)
    {
        dodge = value.isPressed;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
