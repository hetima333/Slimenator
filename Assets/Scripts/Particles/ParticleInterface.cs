using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleInterface : MonoBehaviour {

    ParticleSystem[] _ps;

    private void Start()
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

}
