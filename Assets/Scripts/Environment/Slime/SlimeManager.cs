﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : MonoBehaviour {

    [SerializeField]
    private List<ElementType>
        Elements = new List<ElementType>();

    private static SlimeManager _instance;
    public static SlimeManager Instance { get { return _instance; } }

    [SerializeField]
    private GameObject _slimeList;
    private List<GameObject> _slimePool = new List<GameObject>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {

        ExpandPool();
    }

    private void Update()
    {
        //Temporary
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int random = Random.Range(0, Elements.Count);
            GameObject slime = GetSlimeFromPool(random);
            slime.transform.position = new Vector3(1, 1, 1);
        }
    }

    public GameObject GetSlimeFromPool(int type)
    {
        foreach (GameObject slime_obj in _slimePool)
        {
            if (!slime_obj.activeSelf)
            {
                SlimeBase temp_component = slime_obj.GetComponent<SlimeBase>();
                if (temp_component != null)
                    DestroyImmediate(temp_component);


                System.Type _MyScriptType = System.Type.GetType(Elements[type].GetSlimeScriptName());
                slime_obj.AddComponent(_MyScriptType);

                slime_obj.GetComponent<SlimeBase>().Init(100, 2, Elements[type]);
                slime_obj.SetActive(true);
                return slime_obj;
            }
        }

        ExpandPool();

        return GetSlimeFromPool(type);
    }

    private void ExpandPool()
    {
        for (int i = 0; i < 5; ++i)
        {
            GameObject obj = null;
            obj = Instantiate(_slimeList);
            obj.SetActive(false);
            _slimePool.Add(obj);
            obj.transform.parent = gameObject.transform;
        }
    }
}
