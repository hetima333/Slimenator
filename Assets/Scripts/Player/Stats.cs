using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity/Stats")]
public class Stats : ScriptableObject
{
    [SerializeField]
    private GameObject
        _Prefab;

    [SerializeField]
    private float
        _MaxHealth,
        _MaxHealthMultiplyer,
        _Health,

        _MaxDamage,
        _MaxDamageMultiplyer,

        _MaxSpeed,
        _MaxSpeedMultiplyer,

        _SuckingPower,
        _SuckingPowerMultiplyer;


    [SerializeField]
    private bool
        _UsedByMultipleEntities = true;

    public float MaxHealthProperties
    {
        get
        {
            return _MaxHealth * _MaxHealthMultiplyer;
        }
    }

    public float HealthProperties
    {
        get
        {
            return _Health;
        }

        set
        {
            _Health = value;
        }
    }

    public float DamageProperties
    {
        get
        {
            return _MaxDamage * _MaxDamageMultiplyer;
        }
    }

    public float SpeedProperties
    {
        get
        {
            return _MaxSpeed * _MaxSpeedMultiplyer;
        }
    }

    public float HealthMultiplyerProperties
    {
        get
        {
            return _MaxHealthMultiplyer;
        }

        set
        {
            _MaxHealthMultiplyer = Mathf.Clamp(value, 0, 100);
        }
    }

    public float DamageMultiplyerProperties
    {
        get
        {
            return _MaxDamageMultiplyer;
        }

        set
        {
            _MaxDamageMultiplyer = Mathf.Clamp(value, 0, 100);
        }
    }

    public float SpeedMultiplyerProperties
    {
        get
        {
            return _MaxSpeedMultiplyer;
        }

        set
        {
            _MaxSpeedMultiplyer = Mathf.Clamp(value, 0, 100);
        }
    }

    public float SuckingPowerProperties
    {
        get
        {
            return _SuckingPower * _SuckingPowerMultiplyer;
        }
    }

    public float SuckingPowerMultiplyerProperties
    {
        get
        {
            return _SuckingPowerMultiplyer;
        }

        set
        {
            _SuckingPowerMultiplyer = Mathf.Clamp(value, 0, 100);
        }
    }

    public bool IsUseByMultiple
    {
        get
        {
            return _UsedByMultipleEntities;
        }
    }

    public string GetPrefabName()
    {
        return _Prefab.name;
    }
}
