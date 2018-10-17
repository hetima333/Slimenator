using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Target Type/AOE Self")]
public class SkillTargetAOE : SkillCastingType
{
    [SerializeField]
    private float
        _Range;

    public override List<GameObject> GetTargets(GameObject caster)
    {
        return null;
    }
}
