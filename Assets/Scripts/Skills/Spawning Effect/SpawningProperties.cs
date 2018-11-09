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
     _Particle;

    [SerializeField]
    private float
        _Radius, 
        _Delay,
        _YOffset, 
        _Damage;

    [SerializeField]
    private EnumHolder.AreaEffectType
       _Type;

    [SerializeField]
    private SOList
        _ElementType;

    [SerializeField]
    protected GameObjectList
        _Objects;

    public GameObject GetParticle()
    {
        return _Particle;
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

    public EnumHolder.AreaEffectType GetEffectType()
    {
        return _Type;
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
