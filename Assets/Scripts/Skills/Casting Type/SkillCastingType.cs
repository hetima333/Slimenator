﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillCastingType : ScriptableObject
{
    public abstract List<GameObject> GetTargets(ref Vector3 caster_position, ref SkillTier tier, ref GameObjectList targets, ref GameObject caster);
}
