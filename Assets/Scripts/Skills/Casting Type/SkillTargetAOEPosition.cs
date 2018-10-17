using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Target Type/AOE On Target")]
public class SkillTargetAOEPosition : SkillCastingType
{
    [SerializeField]
    private float
        _Range;

    public override List<GameObject> GetTargets(GameObject caster)
    {
        return null;
    }
}
