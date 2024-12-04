using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]

public class SetIdlePose : MonoBehaviour
{

    public AnimationClip idleAnimation; // Idle �ִϸ��̼� Ŭ��

    void Update()
    {
        if (idleAnimation != null)
        {
            idleAnimation.SampleAnimation(gameObject, 0f); // ù �������� ����
        }
    }
}