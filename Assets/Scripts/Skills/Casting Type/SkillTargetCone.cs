using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Target Type/Cone")]
public class SkillTargetCone : SkillCastingType
{
    [SerializeField]
    private float
        _Range;

    public override List<GameObject> GetTargets(ref GameObject caster, ref SkillTier tier, ref List<GameObject> targets)
    {

        return null;
    }  
}
