using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Properties/Area Effect")]
public class SpawningProperties : ScriptableObject
{
    [BackgroundColor(0.5f, 0.5f, 0f, 0.5f)]
    [Header("Area Effect Properties")]
    [SerializeField]
    private GameObject
        _StartingParticle,
        _EndingParticle;

    [SerializeField]
    private float
        _Radius, 
        _Delay,
        _YOffset, 
        _Damage;

    [SerializeField]
    private EnumHolder.AreaEffectType
        _StartingType,
        _EndingType;

    [SerializeField]
    private SOList
        _ElementType;

    [Tooltip("Objects to be spawned/affected")]
    [SerializeField]
    protected GameObjectList
        _Objects;

    [Tooltip("[FOR SPAWNING] Tier determines the size and power of spawned Object")]
    [SerializeField]
    public SkillTier
     _startingTier;

    public GameObject GetStartingParticle()
    {
        return _StartingParticle;
    }

    public GameObject GetEndingParticle()
    {
        return _EndingParticle;
    }

    public float GetRadius()
    {
        return _Radius;
    }

    public float GetYOffset()
    {
        return _YOffset;
    }

    public float GetDelay()
    {
        return _Delay;
    }

    public float GetDamage()
    {
        return _Damage;
    }

    public EnumHolder.AreaEffectType GetEffectType(bool IsEnding)
    {
        if(!IsEnding)
            return _StartingType;
        return _EndingType;
    }

    public SkillTier GetTier()
    {
        return _startingTier;
    }

    public GameObjectList GetTargetable()
    {
        return _Objects;
    }

    public SOList GetElement()
    {
        return _ElementType;
    }
}
