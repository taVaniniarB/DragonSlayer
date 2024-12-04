using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class FireVFX : MonoBehaviour
{
    public ParticleSystem[] particles;
    public bool breathing;
    void Update()
    {
        if (breathing)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                if (!particles[i].isPlaying)
                {
                    particles[i].Play();
                }
            }
        }
        else
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Stop();

            }
        }
    }


    public void BreathStart()
    {
        breathing = true;
    }

    public void BreathEnd()
    {
        breathing = false;
    }
}
