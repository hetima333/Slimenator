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
        _classname;

    [SerializeField]
    private GameObject
        _effect;

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
        return _classname;
    }

    public GameObject GetEffect()
    {
        return _effect;
    }
}
