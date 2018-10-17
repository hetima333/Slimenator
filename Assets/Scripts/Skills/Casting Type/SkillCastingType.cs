using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillCastingType : ScriptableObject
{
    private float
       _Multiplyer;

    public abstract List<GameObject> GetTargets(GameObject caster);

    public void SetMultiplyer(float Input)
    {
        _Multiplyer = Input;
    }
}
