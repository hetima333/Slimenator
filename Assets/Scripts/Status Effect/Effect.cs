using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect/Effect")]
public class Effect : ScriptableObject {

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
}
