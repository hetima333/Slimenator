using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Targeted Skill")]
public class SkillTargeted : Skill
{
    [SerializeField]
    private SkillCastingType
        _CastingType;

    public override void Engage(GameObject caster, Vector3 spawn_position = new Vector3(), Vector3 dir = new Vector3())
    {
        base.Engage(caster, spawn_position, dir);

        if (IsTimeOver())
        {
            if (!IsSkillOver() || _UseTimer == 0)
            {
                foreach (GameObject obj in _CastingType.GetTargets(ref spawn_position, ref _SkillTier, _Targetable.GetList(), ref caster))
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
