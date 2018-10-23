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
        _CastLength,
        _Damage;

    [SerializeField]
    [TextArea(15, 20)]
    private string
        _Description;

    [SerializeField]
    private GameObject
      _ChannelingParticle,
      _CastingParticle;

    private GameObject
        _ChannelingParticleCopy,
        _CastingParticleCopy;

    protected SkillTier
        _SkillTier;

    protected float
        _Timer,
        _UseTimer;

    public virtual void Init()
    {
        _Timer = _CastTime;
        _UseTimer = _CastLength;
    }

    public virtual void Engage(GameObject caster, Vector3 dir = new Vector3())
    {
        _Timer -= Time.deltaTime;

        if (_Timer <= 0)
            _UseTimer -= Time.deltaTime;
        else
            _Timer -= Time.deltaTime;

        if (IsTimeOver())
        {
            if (_ChannelingParticleCopy != null)
            {
                DestroyImmediate(_ChannelingParticleCopy);
                _ChannelingParticleCopy = null;
            }
        }
        else
        {
            if (_ChannelingParticle != null && _ChannelingParticleCopy == null)
            {
                _ChannelingParticleCopy = Instantiate(_ChannelingParticle, caster.transform.position, caster.transform.rotation);
            }
        }

        if(!IsSkillOver())
        {
            if (_CastingParticleCopy != null)
            {
                DestroyImmediate(_CastingParticleCopy);
                _CastingParticleCopy = null;
            }
        }
        else
        {
            if (_CastingParticle != null && _CastingParticleCopy == null)
            {
                _CastingParticleCopy = Instantiate(_CastingParticle, caster.transform.position, caster.transform.rotation);
            }
        }
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

    public bool IsSkillOver()
    {
        return _UseTimer <= 0;
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
