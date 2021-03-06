﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Properties/Projectile")]
public class ProjectileProperties : BaseProperties
{
    [BackgroundColor(0.5f, 0.5f, 0f, 0.5f)]
    [Header("Projectile Properties")]
    [SerializeField]
    private GameObject
     _MovingParticle,
     _ImpactParticle;

    [Tooltip("Does it Shake the screen on Impact/Traveling")]
    [SerializeField]
    private bool
       _ShakeOnTraveling,
       _ShakeOnImpact;

    [Tooltip("How often the Shake will occur")]
    [SerializeField]
    private float
        _ShakeScreenDelay;

    [Tooltip("Only needed when Skill does Shake screen")]
    [SerializeField]
    private GameEvent
        _ShakeScreenEvent;

    [SerializeField]
    private float
        _ImpactRadius, 
        _SpawningIteration;

    [Tooltip("Only if the object you are spawning is an [Area Effect] or an oject that needs Element Type Info")]
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

    [Tooltip("Does spawned Object have random motion")]
    [SerializeField]
    public bool
     _HasRandomMotion;

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

    public bool ShakeOnTraveling()
    {
        return _ShakeOnTraveling;
    }

    public bool ShakeOnImpact()
    {
        return _ShakeOnImpact;
    }

    public float GetShakeDelay()
    {
        return _ShakeScreenDelay;
    }

    public GameEvent GetEvent()
    {
        return _ShakeScreenEvent;
    }

    public bool HasRandomPosition()
    {
        return _HasRandomMotion;
    }
}
