using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Target Type/Self")]
public class SkillTargetSelf : SkillCastingType
{
    [SerializeField]
    private float
        _Range;

    public override List<GameObject> GetTargets(ref GameObject caster, ref SkillTier tier, ref List<GameObject> targets)
    {
        List<GameObject> list = new List<GameObject>();
        list.Add(caster);

        return list;
    }
}