using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Properties/Projectile")]
public class ProjectileProperties : ScriptableObject
{
    [SerializeField]
    private GameObject
     _MovingParticle,
     _ImpactParticle;

    [SerializeField]
    private float
        _ImpactRadius;

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
}
