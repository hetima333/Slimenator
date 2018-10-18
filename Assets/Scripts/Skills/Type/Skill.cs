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
    protected List<SkillProperties>
        _Properties = new List<SkillProperties>();

    [SerializeField]
    protected List<GameObject>
        _Targetable = new List<GameObject>();

    [SerializeField]
    protected float
        _CastTime,
        _Damage;

    [SerializeField]
    [TextArea(15, 20)]
    private string
        _Description;

    protected SkillTier
        _SkillTier;

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

    public ElementType[] GetCombinationElements()
    {
        return _Combination;
    }

    public ElementType GetBaseElement()
    {
        return _Base;
    }

    public bool IsTimeOver()
    {
        return _Timer <= 0;
    }

    public void SetSkillTier(SkillTier tier)
    {
        _SkillTier = tier;
    }

    public SkillTier GetSkillTier()
    {
        return _SkillTier;
    }

    public string GetDescription()
    {
        return _Description;
    }
}
