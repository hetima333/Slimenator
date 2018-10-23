using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Target Type/Self")]
public class SkillTargetSelf : SkillCastingType
{
    [SerializeField]
    private float
        _Range;

    public override List<GameObject> GetTargets(ref Vector3 casting_position, ref SkillTier tier, ref List<GameObject> targets, ref GameObject caster)
    {
        List<GameObject> list = new List<GameObject>();
        list.Add(caster);

        return list;
    }
}