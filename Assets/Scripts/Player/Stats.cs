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
        MaxHealth,
        MaxDamage,
        MaxSpeed,

        MaxSpeedMultiplyer,
        MaxDamageMultiplyer,
        MaxHealthMultiplyer,

        Sucking_Power;

    public float HealthProperties
    {
        get
        {
            return MaxHealth;
        }
    }

    public float DamageProperties
    {
        get
        {
            return MaxDamage;
        }
    }

    public float SpeedProperties
    {
        get
        {
            return MaxSpeed;
        }
    }

    public float HealthMultiplyerProperties
    {
        get
        {
            return MaxHealthMultiplyer;
        }

        set
        {
            MaxHealthMultiplyer = Mathf.Clamp(value, 0, 100);
        }
    }

    public float DamageMultiplyerProperties
    {
        get
        {
            return MaxDamageMultiplyer;
        }

        set
        {
            MaxDamageMultiplyer = Mathf.Clamp(value, 0, 100);
        }
    }

    public float SpeedMultiplyerProperties
    {
        get
        {
            return MaxSpeedMultiplyer;
        }

        set
        {
            MaxSpeedMultiplyer = Mathf.Clamp(value, 0, 100);
        }
    }

    public float SuckingPowerProperties
    {
        get
        {
            return Sucking_Power;
        }
    }

    public string GetPrefabName()
    {
        return _Prefab.name;
    }
}
