using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Skills/Targeted Skill")]
public class SkillTargeted : Skill
{
    [BackgroundColor(0f, 1f, 1f, 1f)]
    [Header("Targeted Type Skill")]
    [SerializeField]
    private SkillCastingType
        _CastingType;

    [SerializeField]
    private float
        _AttackDelay;

    private float
        _Timer = 0;

    public override void Engage(GameObject caster, Vector3 spawn_position = new Vector3(), Vector3 dir = new Vector3())
    {
        base.Engage(caster, spawn_position, dir);

        if (IsTimeOver())
        {
            if (!IsSkillOver() || _CastingTimer == 0)
            {
                if (_Timer <= 0)
                {
                    foreach (GameObject obj in _CastingType.GetTargets(ref spawn_position, ref _SkillTier, ref _Targetable, ref caster))
                    {
                        IDamageable dmg = obj.GetComponent<IDamageable>();

                        if (dmg != null)
                        {
                            Debug.DrawLine(spawn_position, obj.transform.position, Color.yellow, 1f);

                            dmg.TakeDamage(_Damage * ((_SkillTier != null) ? _SkillTier.GetMultiplyer() : 1));
                            Debug.Log("[Damaging (" + _Damage * ((_SkillTier != null) ? _SkillTier.GetMultiplyer() : 1) + ")] " + obj.name);

                            if (_StatusEffect.Count > 0)
                            {
                                if (Random.Range(0, 100) < _StatusApplyPercentage * ((_SkillTier != null) ? _SkillTier.GetMultiplyer() : 1))
                                {
                                    foreach (StatusEffect se in _StatusEffect)
                                    {
                                        Debug.Log("[Applying (" + se.name + ")] " + obj.name);
                                        se.GetEvent().InvokeSpecificListner(obj.GetInstanceID());
                                    }
                                }
                            }
                        }
                    }
                    _Timer = _AttackDelay;
                }
                else
                    _Timer -= Time.deltaTime;
            }
        }
    }

    public SkillCastingType GetCastType()
    {
        return _CastingType;
    }
}
