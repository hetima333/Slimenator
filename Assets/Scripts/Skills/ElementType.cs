using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity/Elements")]
public class ElementType : ScriptableObject
{
    [SerializeField]
    private float
        _minAmount,
        _maxAmount;

    [SerializeField]
    private Color
        _color;

    [SerializeField]
    private string
        _ClassName;

    [SerializeField]
    private GameObject
        _ParticleEffect;

    [SerializeField]
    private StatusEffect
        _StatusEffect;

    public int GetRandomAmount()
    {
        return (int)Random.Range(_minAmount, _maxAmount);
    }

    public Color GetColor()
    {
        return _color;
    }

    public string GetSlimeScriptName()
    {
        return _ClassName;
    }

    public GameObject GetEffect()
    {
        return _ParticleEffect;
    }

    public StatusEffect GetStatusEffect()
    {
        return _StatusEffect;
    }
}
