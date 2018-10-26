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
    protected GameObjectList
        _Targetable;

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
        _Timer,
        _UseTimer;

    public virtual void Init()
    {
        if (_ChannelingParticle != null)
        {
            ParticleInterface ChannelingParticlePI = _ChannelingParticle.GetComponent<ParticleInterface>();
            ChannelingParticlePI.Init();
            _Timer = _ChannelingParticle.GetComponent<ParticleInterface>().GetLongestParticleEffect();
        }
        else
            _Timer = 0;

        if (_CastingParticle != null)
        {
            ParticleInterface CastingParticlePI = _CastingParticle.GetComponent<ParticleInterface>();
            CastingParticlePI.Init();
            _UseTimer = _CastingParticle.GetComponent<ParticleInterface>().GetLongestParticleEffect();
        }
        else
            _UseTimer = 0;
    }

    public virtual void Engage(GameObject caster, Vector3 spawn_position = new Vector3(), Vector3 dir = new Vector3())
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
                _ChannelingParticleCopy = Instantiate(_ChannelingParticle, spawn_position, caster.transform.rotation, caster.transform);
                _ChannelingParticleCopy.transform.localScale = new Vector3(_SkillTier.GetMultiplyer(), _SkillTier.GetMultiplyer(), _SkillTier.GetMultiplyer());
            }
        }

        if(IsSkillOver())
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
                _CastingParticleCopy = Instantiate(_CastingParticle, spawn_position, caster.transform.rotation, caster.transform);
                _CastingParticleCopy.transform.localScale = new Vector3(_SkillTier.GetMultiplyer(), _SkillTier.GetMultiplyer(), _SkillTier.GetMultiplyer());
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

    public void Reset()
    {
        if (_ChannelingParticleCopy != null)
        {
            DestroyImmediate(_ChannelingParticleCopy);
            _ChannelingParticleCopy = null;
        }

        if (_CastingParticleCopy != null)
        {
            DestroyImmediate(_CastingParticleCopy);
            _CastingParticleCopy = null;
        }
    }
}
