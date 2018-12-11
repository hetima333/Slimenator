using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Skills/Spawning Skill")]
public class SkillSpawning : Skill
{
    [BackgroundColor(1f, 1f, 0f, 0.5f)]
    [Header("Spawning Type Skill")]
    [SerializeField]
    private SkillCastingType
        _CastingType;

    [SerializeField]
    private float
        _SpawningDelay;

    [SerializeField]
    private uint
        _Iteration;

    [SerializeField]
    private SpawningProperties
        _AreaEffectProperties;

    private float
        _Timer = 0;

    [SerializeField]
    private GameObject
        _SpawnedObject;

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
                        for (int i = 0; i < _Iteration; ++i)
                        {
                            Debug.DrawLine(spawn_position, obj.transform.position, Color.yellow, 1f);

                            GameObject temp = ObjectManager.Instance.InstantiateWithObjectPooling(_SpawnedObject, obj.transform.position, obj.transform.rotation);

                            if (temp.GetComponent<AreaEffect>() != null)
                            {
                                temp.GetComponent<AreaEffect>().Init(_AreaEffectProperties);
                            }
                        }
                    }
                    _Timer = _SpawningDelay;
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
