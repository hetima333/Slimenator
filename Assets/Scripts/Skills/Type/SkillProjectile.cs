using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Projectile Skill")]
public class SkillProjectile : Skill
{
    [BackgroundColor(1f, 0f, 1f, 0.5f)]
    [Header("Projectile Type Skill")]
    [Tooltip("Type of Projectile to fire")]
    [SerializeField]
    private GameObject
        _Projectile;

    [Tooltip("Projectile's Properties")]
    [SerializeField]
    private ProjectileProperties
        _properties;

    [Tooltip("How long it can travel for [Tier will multiply the range]")]
    [SerializeField]
    private float
        _Range;

    [Tooltip("How fast the projectile travels")]
    [SerializeField]
    private float
        _Speed;

    public override void Engage(GameObject caster, Vector3 spawn_position = new Vector3(), Vector3 dir = new Vector3())
    {
        base.Engage(caster, spawn_position, dir);

        if (IsTimeOver())
        {
            if (!IsSkillOver() || _CastingTimer == 0)
            {
                float multiplyer = ((_SkillTier != null) ? _SkillTier.GetMultiplyer() : 1);

                GameObject temp = ObjectManager.Instance.InstantiateWithObjectPooling(_Projectile, spawn_position, caster.transform.rotation);
                temp.GetComponent<Projectile>().Init(dir, _Speed, _properties, _StatusEffect, _Targetable, _Range * multiplyer, _Damage * multiplyer, multiplyer);
                temp.transform.localScale = new Vector3(multiplyer, multiplyer, multiplyer);
            }
        }
    }
}