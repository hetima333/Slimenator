using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleInterface : MonoBehaviour {

    ParticleSystem[] _ps;

    public void Init()
    {
        _ps = GetComponentsInChildren<ParticleSystem>();
    }

    public Vector3 GetParticleEffectSize()
    {
        return transform.localScale;
    }

    public void SetParticleEffectSize(float size)
    {
        transform.localScale = Vector3.one * size;
    }

    public void SetParticleEffectRotation(Quaternion rot)
    {
        transform.localRotation = rot;
    }

    public Quaternion GetParticleEffectRotation()
    {
        return transform.localRotation;
    }

    public float GetLongestParticleEffect()
    {
        float tempMaxDuration = 0;

        for (int i = 0; i < _ps.Length; ++i)
        {
            ParticleSystem.MainModule main = _ps[i].main;
            float duration = main.duration + main.startDelay.constant;
            if (duration > tempMaxDuration)
                tempMaxDuration = duration;
        }

        return tempMaxDuration;
    }
}
