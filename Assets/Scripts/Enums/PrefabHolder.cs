using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Prefab Holder")]
public class PrefabHolder : ScriptableObject {

    [SerializeField]
    private GameObject _prefab;

    public GameObject GetPrefab()
    {
        return _prefab;
    }
}
