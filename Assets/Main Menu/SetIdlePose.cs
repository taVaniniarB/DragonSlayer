using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]

public class SetIdlePose : MonoBehaviour
{

    public AnimationClip idleAnimation; // Idle 애니메이션 클립

    void Update()
    {
        if (idleAnimation != null)
        {
            idleAnimation.SampleAnimation(gameObject, 0f); // 첫 프레임을 적용
        }
    }
}