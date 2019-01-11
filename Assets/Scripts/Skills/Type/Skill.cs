using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    [BackgroundColor(0.8f, 0.3f, 0.5f, 0.5f)]
    [Header("Audio")]
    [Tooltip("Audio clips, leave empty if no audio should be played")]
    [SerializeField]
    private AudioClip
        _CastingAudio,
        _ChannelingAudio;

    [Tooltip("Tick if audio should loop")]
    [SerializeField]
    private bool
        _IsCastingAudioLoop,
        _IsChannelingAudioLoop;

    [BackgroundColor(0f, 1f, 0f, 0.5f)]
    [Header("Skills Properties")]
    [Tooltip("Skill is created via Combinations")]
    [SerializeField]
    private ElementType[]
         _Combination = new ElementType[3];

    [Tooltip("Skill is created via Base Element")]
    [SerializeField]
    private ElementType
        _Base;

    [Tooltip("Targets that can be targeted")]
    [SerializeField]
    protected GameObjectList
        _Targetable;

    [Tooltip("Status Effect to apply to Target")]
    [SerializeField]
    protected List<StatusEffect>
        _StatusEffect = new List<StatusEffect>();

    [Tooltip("Chance of Status effect being applied to Target [Tier will multiply the chance]")]
    [SerializeField]
    protected float
        _StatusApplyPercentage;

    [Tooltip("Damage Dealt to Target")]
    [SerializeField]
    protected float
        _Damage;

    [Tooltip("Does skill Restrict user Movements")]
    [SerializeField]
    private bool
        _RestrictUserMovement;

    [Tooltip("Does Channeling/Casting Shake the screen")]
    [SerializeField]
    private bool
        _ChannelingShakeScreen, 
        _CastingShakeScreen;

    [Tooltip("How often the Shake will occur")]
    [SerializeField]
    private float
        _ShakeScreenDelay;

    [Tooltip("Only needed when Skill does Shake screen")]
    [SerializeField]
    private GameEvent
        _ShakeScreenEvent;

    [SerializeField]
    [TextArea(15, 20)]
    private string
        _Description;

    [SerializeField]
    private GameObject
      _ChannelingParticle,
      _CastingParticle;

    [Tooltip("If Skill does not have particles")]
    [SerializeField]
    private float
        _ChannelingTime,
        _CastingTime;

    private GameObject
        _ChannelingParticleCopy,
        _CastingParticleCopy;

    protected SkillTier
        _SkillTier;

    protected float
        _ChannelingTimer,
        _CastingTimer, 
        _Multiplyer, 
        _ScreenShakeTimer;

    private bool
        _PlayedChannelingAudio,
        _PlayedCastingAudio;

    [SerializeField]
    private uint
        _AnimationID;

    public virtual void Init()
    {
        _PlayedCastingAudio = _PlayedChannelingAudio = false;

        if (_ChannelingParticle != null)
        {
            ParticleInterface ChannelingParticlePI = _ChannelingParticle.GetComponent<ParticleInterface>();
            ChannelingParticlePI.Init();
            _ChannelingTimer = ChannelingParticlePI.GetLongestParticleEffect();
            //Debug.Log("Channeling Particle: " + _ChannelingTimer);
        }
        else
            _ChannelingTimer = _ChannelingTime;

        if (_CastingParticle != null)
        {
            ParticleInterface CastingParticlePI = _CastingParticle.GetComponent<ParticleInterface>();
            CastingParticlePI.Init();
            _CastingTimer = CastingParticlePI.GetLongestParticleEffect();
            //Debug.Log("Casting Particle: " + _CastingTimer);
        }
        else
            _CastingTimer = _CastingTime;

        _Multiplyer = ((_SkillTier != null) ? _SkillTier.GetMultiplyer() : 1);
        _ScreenShakeTimer = 0;
    }

    public virtual void Engage(GameObject caster, Vector3 spawn_position = new Vector3(), Vector3 dir = new Vector3())
    {
        _ScreenShakeTimer -= Time.deltaTime;

        if (_ChannelingTimer <= 0)
        {
            if (_CastingTimer > 0)
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

            if (_PlayedChannelingAudio)
            {
                if (_IsChannelingAudioLoop)
                {
                    AudioManager.Instance.StopSE(_ChannelingAudio.name);
                    _PlayedChannelingAudio = false;
                }
            }

            if (IsSkillOver())
            {
                if (_CastingParticleCopy != null)
                {
                    Destroy(_CastingParticleCopy);
                    _CastingParticleCopy = null;
                    Debug.Log("---CASTING SKILL---");
                }

                if (_PlayedCastingAudio)
                {
                    if (_IsCastingAudioLoop)
                    {
                        AudioManager.Instance.StopSE(_CastingAudio.name);
                        _PlayedCastingAudio = false;
                    }
                }
				// ギガバキュームの際の応急処置
				if(name == "Giga Vacuum"){
					var player = caster.GetComponent<EntityPlayer>();
					if(player == null){
						return;
					}
					player.Controllable = true;
				}
			}
            else
            {
                if (!_PlayedCastingAudio)
                {
                    if (_CastingAudio != null)
                    {
                        _PlayedCastingAudio = true;
                        AudioManager.Instance.PlaySE(_CastingAudio.name, _IsCastingAudioLoop);
                    }
                }

                if (_ScreenShakeTimer <= 0 && _CastingShakeScreen)
                {
                    for (int i = 0; i < _Multiplyer; ++i)
                        _ShakeScreenEvent.InvokeAllListeners();
                    _ScreenShakeTimer = _ShakeScreenDelay;
                }

                if (_CastingParticle != null && _CastingParticleCopy == null)
                {
                    _CastingParticleCopy = Instantiate(_CastingParticle, spawn_position, caster.transform.rotation, caster.transform);
                    _CastingParticleCopy.transform.localScale = new Vector3(_Multiplyer, _Multiplyer, _Multiplyer);
                    Debug.Log("+++CASTING SKILL+++");

					// ギガバキュームの際の応急処置
					if(name == "Giga Vacuum"){
						var player = caster.GetComponent<EntityPlayer>();
						if(player == null){
							return;
						}
						player.Controllable = false;
					}
                }
            }
        }
        else
        {
            if (_ScreenShakeTimer <= 0 && _ChannelingShakeScreen)
            {
                for (int i = 0; i < _Multiplyer; ++i)
                    _ShakeScreenEvent.InvokeAllListeners();
                _ScreenShakeTimer = _ShakeScreenDelay;
            }

            if (!_PlayedChannelingAudio)
            {
                if (_ChannelingAudio != null)
                {
                    _PlayedChannelingAudio = true;
                    AudioManager.Instance.PlaySE(_ChannelingAudio.name, _IsChannelingAudioLoop);
                }
            }

            if (_ChannelingParticle != null && _ChannelingParticleCopy == null)
            {
                Debug.Log("+++CHANNELING SKILL+++");
                _ChannelingParticleCopy = Instantiate(_ChannelingParticle, spawn_position, caster.transform.rotation, caster.transform);
                _ChannelingParticleCopy.transform.localScale = new Vector3(_Multiplyer, _Multiplyer, _Multiplyer);
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

    public List<StatusEffect> GetStatusEffects()
    {
        return _StatusEffect;
    }

    public string GetDescription()
    {
        return _Description;
    }

    public bool IsMoveOnCast()
    {
        return _RestrictUserMovement;
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

        if (_PlayedCastingAudio)
        {
            if (_IsCastingAudioLoop)
            {
                AudioManager.Instance.StopSE(_CastingAudio.name);
                _PlayedCastingAudio = false;
            }
        }

        if (_PlayedChannelingAudio)
        {
            if (_IsChannelingAudioLoop)
            {
                AudioManager.Instance.StopSE(_ChannelingAudio.name);
                _PlayedChannelingAudio = false;
            }
        }
    }

    public uint GetAnimationID()
    {
        return _AnimationID;
    }
}
