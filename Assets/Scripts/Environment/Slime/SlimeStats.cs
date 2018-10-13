using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStats
{
    public enum Slime_Type
    {
        SLIME_FIRE,
        SLIME_ICE,
        SLIME_LIGHTING,
        SLIME_HEALTH,
        SLIME_GOLD
    };

    float
        _health,
        _maxHealth,
        _velocity,
        _movementRange;

    bool
        _isDead;

    Slime_Type _type;

    #region Getter/Setter
    public float Health
    {
        get
        {
            return _health;
        }

        set
        {
            _health = value;
        }
    }
    public float MaxHealth
    {
        get
        {
            return _maxHealth;
        }

        set
        {
            _maxHealth = value;
        }
    }
    public float Velocity
    {
        get
        {
            return _velocity;
        }

        set
        {
            _velocity = value;
        }
    }
    public float MovementRange
    {
        get
        {
            return _movementRange;
        }

        set
        {
            _movementRange = value;
        }
    }
    public bool IsDead
    {
        get
        {
            return _isDead;
        }

        set
        {
            _isDead = value;
        }
    }
    public Slime_Type Type
    {
        get
        {
            return _type;
        }

        set
        {
            _type = value;
        }
    }

    #endregion
}
