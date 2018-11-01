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
    protected GameObjectList
        _Targetable;

    [SerializeField]
    protected List<StatusEffect>
        _StatusEffect = new List<StatusEffect>();

    [SerializeField]
    protected float
        _StatusApplyPercentage;

    [SerializeField]
    protected float
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
        _ChannelingTimer,
        _CastingTimer;

    public virtual void Init()
    {
        if (_ChannelingParticle != null)
        {
            ParticleInterface ChannelingParticlePI = _ChannelingParticle.GetComponent<ParticleInterface>();
            ChannelingParticlePI.Init();
            _ChannelingTimer = _ChannelingParticle.GetComponent<ParticleInterface>().GetLongestParticleEffect();
            Debug.Log("Channeling Particle: " + _ChannelingTimer);
        }
        else
            _ChannelingTimer = 0;

        if (_CastingParticle != null)
        {
            ParticleInterface CastingParticlePI = _CastingParticle.GetComponent<ParticleInterface>();
            CastingParticlePI.Init();
            _CastingTimer = _CastingParticle.GetComponent<ParticleInterface>().GetLongestParticleEffect();
            Debug.Log("Casting Particle: " + _CastingTimer);
        }
        else
            _CastingTimer = 0;
    }

    public virtual void Engage(GameObject caster, Vector3 spawn_position = new Vector3(), Vector3 dir = new Vector3())
    {
        _ChannelingTimer -= Time.deltaTime;

        if (_ChannelingTimer <= 0)
        {
            if(_CastingTimer > 0)
                _CastingTimer -= Time.deltaTime;
        }
        else
            _ChannelingTimer -= Time.deltaTime;

        if (IsTimeOver())
        {
            if (_ChannelingParticleCopy != null)
            {
                Destroy(_ChannelingParticleCopy);
                _ChannelingParticleCopy = null;
                Debug.Log("---CHANNELING SKILL---");
            }
        }
        else
        {
            if (_ChannelingParticle != null && _ChannelingParticleCopy == null)
            {
                Debug.Log("+++CHANNELING SKILL+++");
                _ChannelingParticleCopy = Instantiate(_ChannelingParticle, spawn_position, caster.transform.rotation, caster.transform);
                _ChannelingParticleCopy.transform.localScale = new Vector3(_SkillTier.GetMultiplyer(), _SkillTier.GetMultiplyer(), _SkillTier.GetMultiplyer());
            }
        }

        if (IsTimeOver())
        {
            if (IsSkillOver())
            {
                if (_CastingParticleCopy != null)
                {
                    Destroy(_CastingParticleCopy);
                    _CastingParticleCopy = null;
                    Debug.Log("---CASTING SKILL---");
                }
            }
            else
            {
                if (_CastingParticle != null && _CastingParticleCopy == null)
                {
                    _CastingParticleCopy = Instantiate(_CastingParticle, spawn_position, caster.transform.rotation, caster.transform);
                    _CastingParticleCopy.transform.localScale = new Vector3(_SkillTier.GetMultiplyer(), _SkillTier.GetMultiplyer(), _SkillTier.GetMultiplyer());
                    Debug.Log("+++CASTING SKILL+++");
                }
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
        return _ChannelingTimer <= 0;
    }

    public bool IsSkillOver()
    {
        return _CastingTimer <= 0;
    }

    public void SetSkillTier(SkillTier tier)
    {
        _SkillTier = tier;
    }

    public void SetElementType(List<ElementType> type)
    {
        foreach(ElementType et in type)
        {
            if(et.GetStatusEffect() != null)
                _StatusEffect.Add(et.GetStatusEffect());
        }
    }

    public SkillTier GetSkillTier()
    {
        return _SkillTier;
    }

    public string GetDescription()
    {
        return _Description;
    }

    public void Reset()
    {
        if (_ChannelingParticleCopy != null)
        {
            Destroy(_ChannelingParticleCopy);
            _ChannelingParticleCopy = null;
        }

        if (_CastingParticleCopy != null)
        {
            Destroy(_CastingParticleCopy);
            _CastingParticleCopy = null;
        }
    }
}
