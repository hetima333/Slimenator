﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour {

    [SerializeField]
    private GameObject
        _prefab;

    [SerializeField]
    private PrefabHolder
        _spawner;

    [SerializeField]
    private float
        _spawnTimer,
        _spawnRate,
        _maxSpawnCount;

    [SerializeField]
    public SOList
      _elements;

    [SerializeField]
    public SkillTier
      _startingTier;

    private bool _isActive = true;


    #region Getter/Setter
    public float SpawnTimer
    {
        get
        {
            return _spawnTimer;
        }
    }
    public float SpawnRate
    {
        get
        {
            return _spawnRate;
        }

        set
        {
            _spawnRate = value;
        }
    }
    #endregion

    private void Start()
    {
        _maxSpawnCount = 10;
    }

    private void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (!_isActive)
            return;

        if (_spawnTimer > _spawnRate)
        {
            if ((ObjectManager.Instance.GetActiveObjects(_spawner.GetPrefab()) != null))
            {
                if (ObjectManager.Instance.GetActiveObjects(_prefab) != null)
                {
                    if (ObjectManager.Instance.GetActiveObjects(_prefab).Count < (_maxSpawnCount * ObjectManager.Instance.GetActiveObjects(_spawner.GetPrefab()).Count))
                    {
                        int random = Random.Range(0, _elements.GetList().Count);
                        GetSlimeFromPool(random, gameObject.transform.position);
                    }
                }
                else
                {
                    int random = Random.Range(0, _elements.GetList().Count);
                    GetSlimeFromPool(random, gameObject.transform.position);
                }
            }
            _spawnTimer = 0;
        }
    }

    public GameObject GetSlimeFromPool(int type, Vector3 position = new Vector3())
    {
        GameObject slime_obj = ObjectManager.Instance.InstantiateWithObjectPooling(_prefab, position);
        Stats temp = EnumHolder.Instance.GetStats(_prefab.name);
        SlimeBase temp_component = slime_obj.GetComponent<SlimeBase>();
        slime_obj.transform.SetParent(gameObject.transform);
        GameStateManager.Instance.AddSlime(slime_obj);

        if (temp_component != null)
            Destroy(temp_component);

        System.Type _MyScriptType = System.Type.GetType(((ElementType)_elements.GetList()[type]).GetSlimeScriptName());
        SlimeBase temp_script = slime_obj.AddComponent(_MyScriptType) as SlimeBase;

        temp_script.Init(temp, ((((ElementType)_elements.GetList()[type]).name.Equals("Lightning")) ? 2 : 1), ((ElementType)_elements.GetList()[type]), _startingTier);
        slime_obj.SetActive(true);

        return slime_obj;
    }

    public void UpdateActive(bool isActive)
    {
        _isActive = isActive;
    }
}
