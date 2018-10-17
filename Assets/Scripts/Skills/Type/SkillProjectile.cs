using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Projectile Skill")]
public class SkillProjectile : Skill
{
    [SerializeField]
    private GameObject
        _Projectile;

    public override void Engage(GameObject caster)
    {
        if (_Timer <= 0)
        {
            ;
        }
    }
}