using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Properties/Projectile")]
public class ProjectileProperties : ScriptableObject
{
    [BackgroundColor(0.5f, 0.5f, 0f, 0.5f)]
    [Header("Projectile Properties")]
    [SerializeField]
    private GameObject
     _MovingParticle,
     _ImpactParticle;

    [SerializeField]
    private float
        _ImpactRadius, 
        _SpawningIteration;

    [Tooltip("Only if the object you are spawning is an [Area Effect]")]
    [SerializeField]
    private SpawningProperties
        _AreaEffectProperties;

    [Tooltip("Object to be spawned on Impact")]
    [SerializeField]
    private GameObjectList
        _SpawningObjects;

    [Tooltip("Tier determines the size and power of spawned Object")]
    [SerializeField]
    public SkillTier
     _startingTier;

    public GameObject GetMovingParticle()
    {
        return _MovingParticle;
    }

    public GameObject GetImpactParticle()
    {
        return _ImpactParticle;
    }

    public float GetImpactRadius()
    {
        return _ImpactRadius;
    }

    public float GetIteration()
    {
        return _SpawningIteration;
    }

    public SpawningProperties GetProperties()
    {
        return _AreaEffectProperties;
    }

    public SkillTier GetTier()
    {
        return _startingTier;
    }

    public GameObjectList GetObjectsToSpawn()
    {
        return _SpawningObjects;
    }
}
