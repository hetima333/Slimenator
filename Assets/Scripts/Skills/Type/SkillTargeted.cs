using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Targeted Skill")]
public class SkillTargeted : Skill
{
    [SerializeField]
    private SkillCastingType
        _CastingType;

    public override void Engage(GameObject caster)
    {
        base.Engage(caster);

        if (IsTimeOver())
        {
            if (!IsSkillOver())
            {
                foreach (GameObject obj in _CastingType.GetTargets(ref caster, ref _SkillTier, ref _Targetable))
                {
                    IDamageable dmg = obj.GetComponent<IDamageable>();

                    if (dmg != null)
                    {
                        dmg.TakeDamage(_Damage);
                    }
                }
            }
        }
    }
}
