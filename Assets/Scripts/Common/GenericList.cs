using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class GenericList<T> : ScriptableObject
{
    [SerializeField]
    private List<T>
        _List;

    public List<T> GetList()
    {
        return _List;
    }
}
