using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Projectile Skill")]
public class SkillProjectile : Skill
{
    [SerializeField]
    private GameObject
        _Projectile;

    public override void Engage(GameObject caster, Vector3 dir = new Vector3())
    {
        base.Engage(caster);

        if (IsTimeOver())
        {
            GameObject temp = ObjectManager.Instance.InstantiateWithObjectPooling(_Projectile, caster.transform.position, caster.transform.rotation);
            temp.GetComponent<Projectile>().Init(dir, 20);
            temp.transform.localScale = new Vector3(GetSkillTier().GetMultiplyer(), GetSkillTier().GetMultiplyer(), GetSkillTier().GetMultiplyer());
        }
    }
}