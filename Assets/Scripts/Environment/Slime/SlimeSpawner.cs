using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour {

    [SerializeField]
    private GameObject
        _spawner,
        _prefab;

    [SerializeField]
    private float
        _spawnTimer,
        _spawnRate,
        _maxSpawnCount;

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
        _maxSpawnCount = 8;
    }

    private void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer > _spawnRate)
        {
            //if ((ObjectManager.Instance.GetActiveObjects(_spawner) != null))
            //{
            //    if (ObjectManager.Instance.GetActiveObjects(_prefab) != null)
            //    {
            //        if (ObjectManager.Instance.GetActiveObjects(_prefab).Count < (_maxSpawnCount * ObjectManager.Instance.GetActiveObjects(_spawner).Count))
            //        {
            //            int random = Random.Range(0, EnumHolder.Instance._elements.Count);
            //            GetSlimeFromPool(random, gameObject.transform.position);
            //        }
            //    }
            //}
            //    }
            //    else
            //    {
            //        int random = Random.Range(0, EnumHolder.Instance._elements.Count);
            //        GetSlimeFromPool(random, gameObject.transform.position);
            //    }
            //}
            int random = Random.Range(0, EnumHolder.Instance._elements.Count);
            GetSlimeFromPool(random, gameObject.transform.position);
            _spawnTimer = 0;
        }
    }

    public GameObject GetSlimeFromPool(int type, Vector3 position = new Vector3())
    {
        GameObject slime_obj = ObjectManager.Instance.InstantiateWithObjectPooling(_prefab, position);
        Stats temp = EnumHolder.Instance.GetStats(_prefab.name);
        SlimeBase temp_component = slime_obj.GetComponent<SlimeBase>();

        if (temp_component != null)
            DestroyImmediate(temp_component);

        System.Type _MyScriptType = System.Type.GetType(EnumHolder.Instance._elements[type].GetSlimeScriptName());
        slime_obj.AddComponent(_MyScriptType);

        slime_obj.GetComponent<SlimeBase>().Init(temp, ((EnumHolder.Instance._elements[type].name.Equals("Lightning")) ? 2 : 1), EnumHolder.Instance._elements[type]);
        slime_obj.SetActive(true);

        return slime_obj;
    }
}
