using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity/Stats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField]
    private float
        MaxHealth,
        Damage,
        Speed,
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
            return Damage;
        }
    }

    public float SpeedProperties
    {
        get
        {
            return Speed;
        }
    }

    public float SuckingPowerProperties
    {
        get
        {
            return Sucking_Power;
        }
    }
}
