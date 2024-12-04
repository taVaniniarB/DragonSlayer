using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShacker : MonoBehaviour
{
    public class CamShakeInfo
    {
        public float time;
        public float frequency;
        public float amplitude;

        public CamShakeInfo(float _time, float _amplitude, float _frequency)
        {
            time = _time;
            frequency = _frequency;
            amplitude = _amplitude;
        }
    }

    CinemachineVirtualCamera virtualCamera;
    CinemachineBasicMultiChannelPerlin noise;
    float defaultAmplitude = 0.5f;
    float defaultFrequency = 0.3f;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        else
            Debug.Log("시네머신 카메라 못 찾음");
    }

    private void SetAmplitude(float amplitude)
    {
        if (noise != null)
        {
            noise.m_AmplitudeGain = amplitude;
        }
        else
            Debug.Log("노이즈 못 찾음");
    }

    private void SetFrequency(float frequency)
    {
        if (noise != null)
        {
            noise.m_FrequencyGain = frequency;
        }
    }
    public void CameraShake(CamShakeInfo _info)
    {
        StartCoroutine(CoCameraShake(_info));
    }

    private IEnumerator CoCameraShake(CamShakeInfo _info)
    {
        Debug.Log("CamShake Called");
        SetFrequency(_info.frequency);
        SetAmplitude(_info.amplitude);
        yield return new WaitForSeconds(_info.time);
        ResetShake();

        Debug.Log("CamShake End");
    }

    private void ResetShake()
    {
        if (noise != null)
        {
            noise.m_AmplitudeGain = defaultAmplitude;
            noise.m_FrequencyGain = defaultFrequency;
        }
    }
}
