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
        if (_Timer <= 0)
        {
            foreach(GameObject obj in _CastingType.GetTargets(caster))
            {

            }

            Debug.Log("WHERE MY OBJ POOL");
        }
    }
}
