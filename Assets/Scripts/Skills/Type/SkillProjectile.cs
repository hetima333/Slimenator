using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Projectile Skill")]
public class SkillProjectile : Skill
{
    [SerializeField]
    private GameObject
        _Projectile;

    [SerializeField]
    private float
        _Range;

    public override void Engage(GameObject caster, Vector3 spawn_position = new Vector3(), Vector3 dir = new Vector3())
    {
        base.Engage(caster, spawn_position, dir);

        if (IsTimeOver())
        {
            GameObject temp = ObjectManager.Instance.InstantiateWithObjectPooling(_Projectile, spawn_position, caster.transform.rotation);
            temp.GetComponent<Projectile>().Init(dir, 20, _Range * GetSkillTier().GetMultiplyer(), _Damage * GetSkillTier().GetMultiplyer());
            temp.transform.localScale = new Vector3(GetSkillTier().GetMultiplyer(), GetSkillTier().GetMultiplyer(), GetSkillTier().GetMultiplyer());
        }
    }
}