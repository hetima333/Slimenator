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
        _movementRange,
        _maxMovementRange;

    bool
        _isDead;

    Slime_Type _type;
    ElementType _elementtype;

    #region Getter/Setter
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
    public float MaxMovementRange
    {
        get
        {
            return _maxMovementRange;
        }

        set
        {
            _maxMovementRange = value;
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

    public ElementType Elementtype
    {
        get
        {
            return _elementtype;
        }

        set
        {
            _elementtype = value;
        }
    }

    #endregion
}
