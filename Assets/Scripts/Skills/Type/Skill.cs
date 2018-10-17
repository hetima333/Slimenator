using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    [SerializeField]
    private ElementType[]
         _Combination = new ElementType[3];

    [SerializeField]
    private ElementType
        _Base;

    [SerializeField]
    private List<SkillProperties>
        _Properties = new List<SkillProperties>();

    [SerializeField]
    private float
        _CastTime;

    protected float
        _Timer;

    public virtual void Init()
    {
        _Timer = _CastTime;
    }

    public virtual void Engage(GameObject caster)
    {
        _Timer -= Time.deltaTime;
    }
}
