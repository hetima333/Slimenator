﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect/Effect")]
public class Effect : ScriptableObject
{
    private float
      _Timer,
      _Timer_delay;

    [SerializeField]
    private float
        _delay;

    [SerializeField]
    private EnumHolder.EffectType
      _EffectType;

    [SerializeField]
    private float
        _Amount;

    public void ResetTimer()
    {
        _Timer_delay = _delay;
    }

    public EnumHolder.EffectType GetEffectType()
    {
        return _EffectType;
    }

    public void UpdateEffect()
    {
        if (_Timer > 0)
            _Timer -= Time.deltaTime;
    }

    public bool IsEffectDone()
    {
        return _Timer <= 0;
    }

    public float GetAmount()
    {
        return _Amount;
    }

    public void SetTimer(float timer)
    {
        _Timer = timer;
    }

    public bool GetDelayOver()
    {
        if(_Timer_delay <= 0)
        {
            ResetTimer();
            return true;
        }

        _Timer_delay -= Time.deltaTime;
        return false;
    }
}
