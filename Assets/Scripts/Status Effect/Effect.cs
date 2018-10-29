using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect/Effect")]
public class Effect : ScriptableObject
{
    private float
      _Timer;

    [SerializeField]
    private EnumHolder.EffectType
      _EffectType;

    [SerializeField]
    private float
        _Amount;

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
}
