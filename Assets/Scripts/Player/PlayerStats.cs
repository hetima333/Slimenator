using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity/Stats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField]
    private float
        Health,
        Damage,
        Speed, 
        Money,
        Sucking_Power;

    public float HealthProperties
    {
        get
        {
            return Health;
        }

        set
        {
            Health = value;
            Health = Mathf.Clamp(Health, 0, 3);
        }
    }

    public float DamageProperties
    {
        get
        {
            return Damage;
        }

        set
        {
            Damage = value;
            Damage = Mathf.Clamp(Damage, 0, Mathf.Infinity);
        }
    }

    public float SpeedProperties
    {
        get
        {
            return Speed;
        }

        set
        {
            Speed = value;
            Speed = Mathf.Clamp(Speed, 0, Mathf.Infinity);
        }
    }

    public float MoneyProperties
    {
        get
        {
            return Money;
        }

        set
        {
            Money = value;
            Money = Mathf.Clamp(Speed, 0, Mathf.Infinity);
        }
    }

    public float SuckingPowerProperties
    {
        get
        {
            return Sucking_Power;
        }

        set
        {
            Sucking_Power = value;
            Sucking_Power = Mathf.Clamp(Speed, 0, Mathf.Infinity);
        }
    }
}
