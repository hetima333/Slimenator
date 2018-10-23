using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Target Type/Cone")]
public class SkillTargetCone : SkillCastingType
{
    [SerializeField]
    private float
        _Range;

    public override List<GameObject> GetTargets(ref Vector3 casting_position, ref SkillTier tier, ref List<GameObject> targets, ref GameObject caster)
    {
        List<GameObject> list = new List<GameObject>();

        foreach (GameObject obj in targets)
        {
            if (ObjectManager.Instance.GetActiveObjects(obj) != null)
            {
                foreach (GameObject entity in ObjectManager.Instance.GetActiveObjects(obj))
                {
                    if (Vector3.Angle(casting_position, (entity.transform.position - casting_position).normalized) < 30 && Vector3.Distance(casting_position, entity.transform.position) < _Range * ((tier != null) ? tier.GetMultiplyer() : 1))
                        list.Add(entity);
                }
            }
        }

        return list;
    }  
}
