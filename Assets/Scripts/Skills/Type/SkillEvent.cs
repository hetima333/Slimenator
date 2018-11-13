using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Skills/Event Skill")]
public class SkillEvent : Skill
{
    [BackgroundColor(0f, 0f, 0f, 0.5f)]
    [Header("Event Type Skill")]
    [SerializeField]
    private SkillCastingType
        _CastingType;

    [SerializeField]
    private List<GameEvent>
       _SingleCallEvents;

    [SerializeField]
    private List<GameEvent>
        _MultipleCallEvents;

    [SerializeField]
    private float
        _CallDelay;

    private float
        _Timer = 0;

    private bool
        _SingleEventCalled = false;

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
                        foreach (GameEvent ge in _MultipleCallEvents)
                        {
                            ge.InvokeSpecificListner(obj.GetInstanceID());
                        }

                        if (!_SingleEventCalled)
                        {
                            foreach (GameEvent ge in _SingleCallEvents)
                            {
                                ge.InvokeSpecificListner(obj.GetInstanceID());
                            }

                            _SingleEventCalled = true;
                        }
                    }
                    _Timer = _CallDelay;
                }
                else
                    _Timer -= Time.deltaTime;
            }
        }
    }
}
