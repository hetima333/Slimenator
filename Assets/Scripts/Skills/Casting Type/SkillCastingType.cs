using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillCastingType : ScriptableObject
{
    public abstract List<GameObject> GetTargets(ref GameObject caster, ref SkillTier tier, ref List<GameObject> targets);
}
