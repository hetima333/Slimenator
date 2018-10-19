﻿using System.Collections;
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
        List<GameObject> list = new List<GameObject>();

        foreach (GameObject obj in targets)
        {
            if (ObjectManager.Instance.GetActiveObjects(obj) != null)
            {
                foreach (GameObject entity in ObjectManager.Instance.GetActiveObjects(obj))
                {
                    if (Vector3.Angle(caster.transform.forward.normalized, (entity.transform.position - caster.transform.position).normalized) < 30 && Vector3.Distance(caster.transform.position, entity.transform.position) < _Range * tier.GetMultiplyer())
                        list.Add(entity);
                }
            }
        }

        return list;
    }  
}
