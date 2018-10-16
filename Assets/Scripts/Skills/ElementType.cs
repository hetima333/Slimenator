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
        Color;

    public int GetRandomAmount()
    {
        return (int)Random.Range(_minAmount, _maxAmount);
    }

    public Color GetColor()
    {
        return Color;
    }
}
